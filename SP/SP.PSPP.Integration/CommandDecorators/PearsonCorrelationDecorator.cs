using System.Collections.Generic;
using System.Linq;

using SP.Extensions;
using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.CommandDecorators
{
    public class PearsonCorrelationDecorator : IAnalyzeCommand
    {
        private readonly IAnalyzeCommand inner;
        private readonly GroupDescriptionComparer comparer;

        public PearsonCorrelationDecorator(IAnalyzeCommand inner)
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
            var tables = GetTables(data).ToList();

            yield return GetHeaders(groups.SelectMany(g => g.Select(t => t.Name)).Distinct(), tables.First());

            for (var i = 0; i < groups.Count; i++)
            {
                yield return GetRow(groups[i], tables[i]);
            }
        }

        private IEnumerable<string> GetRow(IEnumerable<VariableDescription> variables, GroupTable table)
        {
            foreach (var variableDescription in variables)
            {
                yield return variableDescription.TargetValue;
            }

            yield return table.Variable1Value;
            yield return table.Variable1Count;

            yield return table.Variable2Value;
            yield return table.Variable2Count;

            yield return table.SigValue;
        }

        private IEnumerable<string> GetHeaders(
            IEnumerable<string> variables,
            GroupTable mainTable)
        {
            foreach (var variable in variables)
            {
                yield return variable;
            }

            yield return mainTable.Variable1Name + " Correlation";
            yield return mainTable.Variable1Name + " #";

            yield return mainTable.Variable2Name + " Correlation";
            yield return mainTable.Variable2Name + " #";
            
            yield return " Sig. (2-tailed)";
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
            return row.First() != "Table: Correlations";
        }

        protected class GroupTable
        {
            private readonly IEnumerable<IEnumerable<string>> table;

            public GroupTable(IEnumerable<IEnumerable<string>> table)
            {
                this.table = table;
            }

            public string Variable1Name
            {
                get { return table.ElementAt(0).ElementAt(2); }
            }

            public string Variable2Name
            {
                get { return table.ElementAt(0).ElementAt(3); }
            }

            public string Variable1Value
            {
                get { return table.ElementAt(1).ElementAt(2); }
            }

            public string Variable2Value
            {
                get { return table.ElementAt(1).ElementAt(3); }
            }

            public string SigValue
            {
                get { return table.ElementAt(2).ElementAt(3); }
            }

            public string Variable1Count
            {
                get { return table.ElementAt(3).ElementAt(2); }
            }

            public string Variable2Count
            {
                get { return table.ElementAt(3).ElementAt(3); }
            }
        }
    }
}