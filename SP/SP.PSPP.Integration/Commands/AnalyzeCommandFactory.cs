using System;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Commands.Implementations;

namespace SP.PSPP.Integration.Commands
{
    public class AnalyzeCommandFactory
    {
        public static IAnalyzeCommand CreateCommand(AnalyzeType commandType, WorkingDirectory workingDirectory)
        {
            switch (commandType)
            {
                case AnalyzeType.Correlation:
                    return new CorrelationCommand(workingDirectory);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
