using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Constants;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class MeanChanceCommand : AnalyzeCommandBase<MeanChanceConfiguration>
    {
        public MeanChanceCommand(WorkingDirectory directory)
            : base(directory)
        {
        }

        protected override string GetSimpleCommandScript(MeanChanceConfiguration configuration)
        {
            return string.Format(CommandConstants.MeanChanceCommonFormat, configuration.TargetVariable.Name);
        }

        protected override string GetGroupCommandScript(GroupDescription group)
        {
            return string.Format(
                CommandConstants.MeanChanceFilterFormat,
                group.ScriptComparison,
                group.Labels,
                group.TargetVariableName);
        }
    }
}
