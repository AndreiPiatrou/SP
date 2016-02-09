using System;
using System.Collections.Generic;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Commands.Implementations;

namespace SP.PSPP.Integration.Commands
{
    public class AnalyzeCommandFactory
    {
        private static readonly IDictionary<AnalyzeType, Func<WorkingDirectory, IAnalyzeCommand>> Dictionary = new Dictionary<AnalyzeType, Func<WorkingDirectory, IAnalyzeCommand>>
        {
            { AnalyzeType.PearsonCorrelation, directory => new PearsonCorrelationCommand(directory) },
            { AnalyzeType.MiddleMean, directory => new MiddleMeanCommand(directory) }
        };

        public static IAnalyzeCommand CreateCommand(AnalyzeType commandType, WorkingDirectory workingDirectory)
        {
            if (!Dictionary.ContainsKey(commandType))
            {
                throw new NotImplementedException();
            }

            return Dictionary[commandType](workingDirectory);
        }
    }
}
