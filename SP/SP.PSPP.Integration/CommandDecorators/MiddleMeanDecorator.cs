using System.Collections.Generic;
using System.Linq;

using SP.Extensions;
using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.CommandDecorators
{
    public class MiddleMeanDecorator : IAnalyzeCommand
    {
        private readonly IAnalyzeCommand inner;
        private readonly GroupDescriptionComparer comparer;

        public MiddleMeanDecorator(IAnalyzeCommand inner)
        {
            this.inner = inner;

            comparer = new GroupDescriptionComparer();
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
            var groups =
                ExtractGroupCombinations(inputData.Configuration.GroupVariables.ToList())
                    .Distinct(comparer)
                    .ToList();
            var tables = GetTables(data);
            var tableGroups = tables.GroupBy(t => t.Name).ToList();

            yield return GetHeaders(groups.SelectMany(g => g.Select(t => t.Name)).Distinct(), tableGroups);

            for (var i = 0; i < groups.Count; i++)
            {
                var values = tableGroups.Select(g => g.ElementAt(i));

                yield return GetRow(groups[i], values);
            }
        }

        private IEnumerable<string> GetRow(IEnumerable<VariableDescription> variables, IEnumerable<GroupTable> tables)
        {
            foreach (var variableDescription in variables)
            {
                yield return variableDescription.TargetValue;
            }

            foreach (var groupTable in tables)
            {
                yield return groupTable.Count;
                yield return groupTable.Mean;
            }
        }

        private IEnumerable<string> GetHeaders(
            IEnumerable<string> variables,
            IList<IGrouping<string, GroupTable>> tables)
        {
            foreach (var variable in variables)
            {
                yield return variable;
            }

            foreach (var table in tables)
            {
                yield return table.Key + " #";
            }

            foreach (var table in tables)
            {
                yield return table.Key + " Middle Mean";
            }
        }

        private IEnumerable<GroupTable> GetTables(OutputData data)
        {
            var result = new List<GroupTable>();
            var table = new List<List<string>>();

            foreach (var row in data.Rows.ToCompleteList().Where(IsMeaningRow))
            {
                if (row.IsEmptyStringCollection() && table.Any())
                {
                    result.Add(new GroupTable(table.ToList()));
                    table.Clear();

                    continue;
                }

                table.Add(row.ToList());
            }

            result.Add(new GroupTable(table.ToList()));
            table.Clear();

            return result;
        }

        private IEnumerable<IEnumerable<VariableDescription>> ExtractGroupCombinations(IList<VariableDescription> groupVariables)
        {
            for (var i = 0; i < groupVariables.First().Values.Count(); i++)
            {
                var i1 = i;
                yield return
                    groupVariables.Select(
                        g =>
                        new VariableDescription(
                            g.Index,
                            g.Name,
                            g.IsNumeric,
                            Enumerable.Empty<string>(),
                            g.Values.ElementAt(i1)));
            }
        }

        private bool IsMeaningRow(IList<string> row)
        {
            return !row.First().StartsWith("Table: Valid cases = ");
        }

        protected class GroupTable
        {
            private readonly IEnumerable<IEnumerable<string>> table;

            public GroupTable(IEnumerable<IEnumerable<string>> table)
            {
                this.table = table;
            }

            public string Name
            {
                get { return table.ElementAt(1).ElementAt(0); }
            }

            public string Mean
            {
                get { return table.ElementAt(1).ElementAt(2); }
            }

            public string Count
            {
                get { return table.ElementAt(1).ElementAt(1); }
            }
        }
    }
}