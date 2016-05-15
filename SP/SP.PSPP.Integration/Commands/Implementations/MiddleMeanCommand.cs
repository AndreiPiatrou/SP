using System.Linq;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Constants;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class MiddleMeanCommand : AnalyzeCommandBase<MiddleMeanConfiguration>
    {
        public MiddleMeanCommand(WorkingDirectory directory)
            : base(directory)
        {
        }

        protected override string GetSimpleCommandScript(MiddleMeanConfiguration configuration)
        {
            return string.Format(CommandConstants.MiddleMeanCommonFormat, configuration.TargetVariables.First().Name);
        }

        protected override string GetGroupCommandScript(GroupDescription @group)
        {
            return string.Format(
                CommandConstants.MiddleMeanFilterFormat,
                group.ScriptComparison,
                group.Labels,
                group.TargetVariable.Name);
        }
    }
}
