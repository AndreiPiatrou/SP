using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using SP.Extensions;
using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;

namespace SP.PSPP.Integration.CommandDecorators
{
    public class MeanChanceDecorator : IAnalyzeCommand
    {
        private readonly IAnalyzeCommand inner;

        public MeanChanceDecorator(IAnalyzeCommand inner)
        {
            this.inner = inner;
        }

        public OutputData Analyze(InputData inputData)
        {
            return new OutputData
            {
                Rows = Aggregate(inner.Analyze(inputData))
            };
        }

        private IEnumerable<IEnumerable<string>> Aggregate(OutputData data)
        {
            var tables = GetTables(data).ToList();
            return Enumerable.Repeat(tables[0].Headers, 1).Concat(tables.SelectMany(t => t.Rows));
        }

        private IEnumerable<GroupTable> GetTables(OutputData data)
        {
            var result = new List<GroupTable>();
            var table = new List<List<string>>();

            foreach (var enumerable in data.Rows.Select(row => row as IList<string> ?? row.ToList()))
            {
                if (enumerable.First().StartsWith("Table: "))
                {
                    table.Add(enumerable.ToList());

                    continue;
                }

                if (enumerable.First() == "Value Label")
                {
                    continue;
                }

                if (enumerable.First() == "Total" && table.Any())
                {
                    result.Add(new GroupTable(table.ToList()));
                    table.Clear();

                    continue;
                }

                if (!enumerable.IsEmptyStringCollection())
                {
                    table.Add(enumerable.ToList());
                }
            }

            return result;
        }

        protected class GroupTable
        {
            private readonly IEnumerable<IEnumerable<string>> table;

            public GroupTable(IEnumerable<IEnumerable<string>> table)
            {
                this.table = table;
            }

            public IEnumerable<GroupPart> Groups
            {
                get
                {
                    return table.Skip(1).First().SelectMany(
                        c =>
                            {
                                return
                                    Regex.Split(c, " & ")
                                        .Where(s => !string.IsNullOrEmpty(s) && s.Contains("="))
                                        .Select(
                                            g =>
                                            new GroupPart(Regex.Split(g, "=")[0], Regex.Split(g, "=")[1].Trim('\'')))
                                        .Distinct();
                            });
                }
            }

            public IEnumerable<string> Headers
            {
                get
                {
                    foreach (var groupPart in Groups)
                    {
                        yield return groupPart.Name;
                    }

                    yield return VariableName;
                    yield return "Frequency";
                    yield return "Percent";
                }
            }

            public IEnumerable<IEnumerable<string>> Rows
            {
                get
                {
                    var groups = Groups.ToList();

                    for (var i = 1; i < table.Count(); i++)
                    {
                        yield return groups.Select(g => g.Value.Trim()).Concat(
                            new[]
                                {
                                    table.ElementAt(i).ElementAt(1).Trim(),
                                    table.ElementAt(i).ElementAt(2).Trim(),
                                    table.ElementAt(i).ElementAt(3).Trim()
                                });
                    }
                }
            }

            public string VariableName
            {
                get { return table.First().First().TrimStart("Table: ".ToCharArray()); }
            }
        }

        protected class GroupPart
        {
            public GroupPart(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; private set; }

            public string Value { get; private set; }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                }
            }

            public override bool Equals(object obj)
            {
                var right = (GroupPart)obj;

                return Name == right.Name && Value == right.Value;
            }

            protected bool Equals(GroupPart other)
            {
                return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
            }
        }
    }
}
