using System.Linq;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Constants;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class PearsonCorrelationCommand : AnalyzeCommandBase<PearsonCorrelationConfiguration>
    {
        public PearsonCorrelationCommand(WorkingDirectory directory)
            : base(directory)
        {
        }

        protected override string GetSimpleCommandScript(PearsonCorrelationConfiguration configuration)
        {
            return string.Format(CommandConstants.PearsonCorrelationCommonFormat, configuration.TargetVariables.First().Name);
        }

        protected override string GetGroupCommandScript(GroupDescription @group)
        {
            return GetGroupCommandScript(group as PearsonCorrelationGroupDescription);
        }

        private string GetGroupCommandScript(PearsonCorrelationGroupDescription group)
        {
            return string.Format(
                CommandConstants.PearsonCorrelationFilterFormat,
                group.ScriptComparison,
                group.Labels,
                group.TargetVariableNames);
        }
    }
}
