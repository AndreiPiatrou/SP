using System;

using SP.FIleSystem.Directory;
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
            throw new NotImplementedException();
        }

        protected override string GetGroupCommandScript(GroupDescription @group)
        {
            throw new NotImplementedException();
        }
    }
}
