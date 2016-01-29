using System;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Constants;
using SP.PSPP.Integration.Models;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class CorrelationCommand : AnalyzeCommandBase
    {
        public CorrelationCommand(WorkingDirectory directory) : base(directory)
        {
        }

        public override OutputData Analyze(InputData inputData)
        {
            var inputFilePath = CreateInputDataFile(inputData);
            var outputFilePath = Directory.GenerateNewFilePath();
            var scriptPath = CreateScriptFile(CreateScript(inputData, inputFilePath));
            var arguments = CreateArguments(scriptPath, outputFilePath);
            var result = ExecuteScript(arguments);

            if (!result.Successed)
            {
                throw new Exception(result.Message);
            }

            return ReadOutputData(outputFilePath);
        }

        private string CreateScript(InputData inputData, string inputFilePath)
        {
            return string.Format(
                CommandConstants.CorrelationCommandFormat,
                inputFilePath,
                string.Join(Environment.NewLine, inputData.Variables),
                string.Join(Environment.NewLine, inputData.Variables));
        }
    }
}
