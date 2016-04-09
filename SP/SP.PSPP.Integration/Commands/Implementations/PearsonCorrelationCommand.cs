using System;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class PearsonCorrelationCommand : AnalyzeCommandBase<PearsonCorrelationConfiguration>
    {
        public PearsonCorrelationCommand(WorkingDirectory directory)
            : base(directory)
        {
        }

        protected override string GetCommandScript(InputData inputData, PearsonCorrelationConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
