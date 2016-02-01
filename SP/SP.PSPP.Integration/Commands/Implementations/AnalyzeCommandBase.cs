using System;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Services;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public abstract class AnalyzeCommandBase<T> : IAnalyzeCommand
    {
        protected const string DefaultArgumentsFormat = " \"{0}\" -o \"{1}\"";
        protected readonly WorkingDirectory Directory;

        protected AnalyzeCommandBase(WorkingDirectory directory)
        {
            Directory = directory;
        }

        public virtual OutputData Analyze(InputData inputData)
        {
            var inputFilePath = CreateInputDataFile(inputData);
            var outputFilePath = Directory.GenerateNewFilePath();
            var scriptPath = CreateScriptFile(GetScript(inputData, (T)inputData.Configuration, inputFilePath));
            var arguments = CreateArguments(scriptPath, outputFilePath);
            var result = ExecuteScript(arguments);

            if (!result.Successed)
            {
                throw new Exception(result.Message);
            }

            return ReadOutputData(outputFilePath);
        }

        protected abstract string GetScript(InputData inputData, T configuration, string inputFilePath);

        protected string CreateInputDataFile(InputData inputData)
        {
            return new InputDataWriter().WriteToFile(inputData, Directory.GenerateNewFilePath());
        }

        protected string CreateScriptFile(string script)
        {
            return Directory.WriteToNewFile(script, ".sps");
        }

        protected StatisticProcessExecutionResult ExecuteScript(string arguments)
        {
            var runner = new StatisticProcessExecuter(arguments);

            return runner.Execute();
        }

        protected string CreateArguments(string inputFilePath, string outputFilePath)
        {
            return string.Format(DefaultArgumentsFormat, inputFilePath, outputFilePath);
        }

        protected OutputData ReadOutputData(string filePath)
        {
            return new OutputDataReader().Read(filePath);
        }
    }
}
