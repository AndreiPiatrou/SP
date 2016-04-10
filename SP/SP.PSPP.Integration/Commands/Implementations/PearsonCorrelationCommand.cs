using System;

using SP.FIleSystem.Directory;
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
            throw new NotImplementedException();
        }

        protected override string GetGroupCommandScript(GroupDescription @group)
        {
            throw new NotImplementedException();
        }
    }
}
