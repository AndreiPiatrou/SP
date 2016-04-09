using System.Text;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Constants;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class MeanChanceCommand : AnalyzeCommandBase<MeanChanceConfiguration>
    {
        public MeanChanceCommand(WorkingDirectory directory)
            : base(directory)
        {
        }

        protected override string GetCommandScript(InputData inputData, MeanChanceConfiguration configuration)
        {
            var groups = configuration.GetGroups(inputData.Rows);
            var builder = new StringBuilder();

            foreach (var group in groups)
            {
                builder.AppendFormat(
                    CommandConstants.MeanChanceFilterFormat,
                    group.ScriptComparison,
                    group.Labels,
                    group.TargetVariableName);

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
