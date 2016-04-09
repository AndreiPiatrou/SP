using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class GroupDescription
    {
        public GroupDescription(
            IList<VariableDescription> variables,
            IList<string> values,
            IEnumerable<string> targetVariableValues,
            string targetVariableName)
        {
            Variables = variables;
            Values = values;
            TargetVariableValues = targetVariableValues;
            TargetVariableName = targetVariableName;
        }

        public IList<VariableDescription> Variables { get; private set; }

        public IEnumerable<string> TargetVariableValues { get; private set; }

        public string TargetVariableName { get; private set; }

        public IList<string> Values { get; private set; }

        public string Labels
        {
            get
            {
                var label = string.Join(" & ", Variables.Select((v, i) => v.Name + "=" + Values[i]));
                var builder = new StringBuilder(TargetVariableName);
                foreach (var targetVariableValue in TargetVariableValues)
                {
                    builder.AppendFormat(" '{0}' '{1}'", targetVariableValue, label);
                }

                return builder.ToString();
            }
        }

        public string ScriptComparison
        {
            get
            {
                var builder = new StringBuilder();

                for (var i = 0; i < Variables.Count; i++)
                {
                    builder.Append(
                        "( " + Variables[i].Name + " = " + GetVariableValue(Values[i], Variables[i].IsNumeric) + " )");

                    if (i != Variables.Count - 1)
                    {
                        builder.Append(" AND ");
                    }
                }

                return builder.ToString();
            }
        }

        private string GetVariableValue(string value, bool isNumeric)
        {
            return isNumeric ? value : "'" + value + "'";
        }
    }
}