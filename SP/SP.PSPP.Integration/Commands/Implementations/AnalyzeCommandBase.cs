using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Services;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class AnalyzeCommandBase : IAnalyzeCommand
    {
        protected const string DefaultArgumentsFormat = " \"{0}\" -o \"{1}\"";
        protected readonly WorkingDirectory Directory;

        public AnalyzeCommandBase(WorkingDirectory directory)
        {
            Directory = directory;
        }

        public virtual OutputData Analyze(InputData inputData)
        {
            throw new System.NotImplementedException();
        }

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
