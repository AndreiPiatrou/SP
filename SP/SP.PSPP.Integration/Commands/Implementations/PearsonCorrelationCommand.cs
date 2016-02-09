using System;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Constants;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class PearsonCorrelationCommand : AnalyzeCommandBase<PearsonCorrelationConfiguration>
    {
        public PearsonCorrelationCommand(WorkingDirectory directory) : base(directory)
        {
        }

        protected override string GetScript(InputData inputData, PearsonCorrelationConfiguration configuration, string inputFilePath)
        {
            return string.Format(
                CommandConstants.PearsonCorrelationCommandFormat,
                inputFilePath,
                string.Join(" F4" + Environment.NewLine, configuration.Variables),
                string.Join(Environment.NewLine, configuration.Variables));
        }
    }
}
