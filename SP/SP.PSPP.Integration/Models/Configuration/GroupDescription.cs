using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class GroupDescription
    {
        public GroupDescription(
            IList<VariableDescription> variables,
            VariableDescription targetVariable)
        {
            Variables = variables;
            TargetVariable = targetVariable;
        }

        public IList<VariableDescription> Variables { get; private set; }

        public VariableDescription TargetVariable { get; private set; }

        public string Labels
        {
            get
            {
                var label = string.Join(" & ", Variables.Select((v, i) => v.Name + "=" + v.TargetValue));
                var builder = new StringBuilder(TargetVariable.Name);

                foreach (var targetVariableValue in TargetVariable.Values)
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
                        "( " + Variables[i].Name + " = " + GetVariableValue(Variables[i].TargetValue, Variables[i].IsNumeric) + " )");

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

    public class PearsonCorrelationGroupDescription : GroupDescription
    {
        public PearsonCorrelationGroupDescription(
            IList<VariableDescription> variables,
            IList<VariableDescription> targetVariables)
            : base(variables, targetVariables.FirstOrDefault())
        {
            TargetVariables = targetVariables;
        }

        public IList<VariableDescription> TargetVariables { get; private set; }

        public string TargetVariableNames
        {
            get
            {
                var builder = new StringBuilder();

                foreach (var variableDescription in TargetVariables)
                {
                    builder.Append(variableDescription.Name);
                    builder.Append(" ");
                }

                return builder.ToString().Trim();
            }
        }
    }
}