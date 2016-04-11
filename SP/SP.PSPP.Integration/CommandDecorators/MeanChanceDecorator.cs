using System;
using System.Collections.Generic;
using System.Globalization;
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
            var rows = tables.SelectMany(t => t.Rows).ToCompleteList();

            yield return tables[0].Headers;

            foreach (var row in UpdatePercent(rows))
            {
                yield return row;
            }
        }

        private IEnumerable<List<string>> UpdatePercent(List<List<string>> rows)
        {
            var totalCount = rows.Sum(t => Convert.ToInt32(t[t.Count - 2]));

            foreach (var row in rows)
            {
                row[row.Count - 1] = (Convert.ToInt32(row[row.Count - 2]) * 100d / totalCount).ToString("F");
            }

            return rows;
        }

        private IEnumerable<GroupTable> GetTables(OutputData data)
        {
            var result = new List<GroupTable>();
            var table = new List<List<string>>();

            foreach (var row in data.Rows.Select(r => r.ToList()).Where(IsMeaningRow))
            {
                if (row.First() == "Total" && table.Any())
                {
                    result.Add(new GroupTable(table.ToList()));
                    table.Clear();

                    continue;
                }

                table.Add(row.ToList());
            }

            return result;
        }

        private bool IsMeaningRow(IList<string> row)
        {
            return !row.IsEmptyStringCollection() && row.ElementAt(0) != "Value Label";
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
                    var groupCell = table.Skip(1).First().First();
                    return groupCell.Contains(" & ")
                               ? Regex.Split(groupCell, " & ").Select(GetGroupPart).Distinct()
                               : Enumerable.Empty<GroupPart>();
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
                                    string.Empty
                                });
                    }
                }
            }

            public string VariableName
            {
                get { return table.First().First().TrimStart("Table: ".ToCharArray()); }
            }

            private GroupPart GetGroupPart(string groupPartString)
            {
                var parts = Regex.Split(groupPartString.Trim(), "=");

                return new GroupPart(parts[0], parts[1]);
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
