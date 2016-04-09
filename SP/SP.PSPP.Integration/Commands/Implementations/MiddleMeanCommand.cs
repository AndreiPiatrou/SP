using System;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class MiddleMeanCommand : AnalyzeCommandBase<MiddleMeanConfiguration>
    {
        public MiddleMeanCommand(WorkingDirectory directory)
            : base(directory)
        {
        }

        protected override string GetCommandScript(InputData inputData, MiddleMeanConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
