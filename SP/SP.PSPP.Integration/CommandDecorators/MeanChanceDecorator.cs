using System;
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
        private readonly EnumerableComparer comparer;

        public MeanChanceDecorator(IAnalyzeCommand inner)
        {
            this.inner = inner;

            comparer = new EnumerableComparer();
        }

        public OutputData Analyze(InputData inputData)
        {
            return new OutputData
            {
                Rows = Aggregate(inner.Analyze(inputData), inputData)
            };
        }

        private IEnumerable<IEnumerable<string>> Aggregate(OutputData data, InputData inputData)
        {
            var targetValues = inputData.Configuration.TargetVariables.ToDictionary(v => v.Name, v => v.TargetValue);
            var tables = GetTables(data).Select(t => t.Filter(targetValues)).Where(t => t.IsFilled).ToList();

            yield return tables.SelectMany(t => t.Headers).Distinct();

            foreach (var row in ExtractRows(tables))
            {
                yield return row;
            }
        }

        private IEnumerable<IEnumerable<string>> ExtractRows(IEnumerable<GroupTable> tables)
        {
            var groupTables = tables as IList<GroupTable> ?? tables.ToList();

            var groups = groupTables.GroupBy(t => t.GroupString, t => t.Groups).ToDictionary(g => g.Key, g => g.First());
            var variableNames = groupTables.Select(t => t.VariableName).Distinct().ToList();

            return
                groups.Select(
                    g =>
                    g.Value.Select(g1 => g1.Value)
                        .Concat(variableNames.SelectMany(v => GetVariables(groupTables, g.Key, v))));
        }

        private IEnumerable<string> GetVariables(IEnumerable<GroupTable> tables, string groupName, string variableName)
        {
            var table = tables.FirstOrDefault(t => t.VariableName == variableName && t.GroupString == groupName);
            if (table == null)
            {
                return new[]
                           {
                               "0",
                               "0"
                           };
            }

            return table.VariableRows.First();
        }

        private IEnumerable<GroupTable> GetTables(OutputData data)
        {
            var result = new List<GroupTable>();
            var table = new List<List<string>>();

            foreach (var row in data.Rows.Select(r => r.ToList()).Where(IsMeaningRow))
            {
                if (row.First() == "Total" && table.Any())
                {
                    result.Add(new GroupTable(table.ToList(), Convert.ToInt32(row[2])));
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
            private readonly int total;

            public GroupTable(IEnumerable<IEnumerable<string>> table, int total)
            {
                this.table = table;
                this.total = total;
            }

            public bool IsFilled
            {
                get { return table.Count() > 1; }
            }

            public IEnumerable<GroupPart> Groups
            {
                get
                {
                    var groupCell = GroupString;
                    return groupCell.Contains(" & ")
                               ? Regex.Split(groupCell, " & ").Select(GetGroupPart).Distinct()
                               : Enumerable.Empty<GroupPart>();
                }
            }

            public string GroupString
            {
                get { return table.Skip(1).First().First(); }
            }

            public IEnumerable<string> Headers
            {
                get
                {
                    foreach (var groupPart in Groups)
                    {
                        yield return groupPart.Name;
                    }

                    yield return VariableName + " - '" + table.ElementAt(1).ElementAt(1) + "' #";
                    yield return VariableName + " - '" + table.ElementAt(1).ElementAt(1) + "' %";
                }
            }

            public IEnumerable<IEnumerable<string>> Rows
            {
                get
                {
                    var groups = Groups.ToList();

                    for (var i = 1; i < table.Count(); i++)
                    {
                        yield return groups.Select(g => g.Value).Concat(
                            new[]
                                {
                                    table.ElementAt(i).ElementAt(2),
                                    string.Empty
                                });
                    }
                }
            }

            public IEnumerable<IEnumerable<string>> VariableRows
            {
                get
                {
                    for (var i = 1; i < table.Count(); i++)
                    {
                        var countCell = table.ElementAt(i).ElementAt(2);
                        yield return new[]
                                         {
                                             countCell,
                                             (Convert.ToInt32(countCell) * 100d / total).ToString("F")
                                         };
                    }
                }
            }

            public string VariableName
            {
                get { return table.First().First().TrimStart("Table: ".ToCharArray()); }
            }

            public GroupTable Filter(IDictionary<string, string> targetValues)
            {
                return new GroupTable(
                    table.Where(
                        r =>
                            {
                                var enumerable = r as string[] ?? r.ToArray();
                                return enumerable.Length == 1 ||
                                       string.Equals(
                                           enumerable.ElementAt(1),
                                           targetValues[VariableName],
                                           StringComparison.CurrentCultureIgnoreCase);
                            }),
                    total);
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
