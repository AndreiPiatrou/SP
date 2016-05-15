using System;
using System.Text;

using SP.Extensions;
using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Constants;
using SP.PSPP.Integration.Extensions;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;
using SP.PSPP.Integration.Services;

namespace SP.PSPP.Integration.Commands
{
    public abstract class AnalyzeCommandBase<T> : IAnalyzeCommand where T : IConfiguration
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
            var fullCommandScript = GetFullCommand(inputFilePath, inputData);
            var scriptPath = CreateScriptFile(fullCommandScript);
            var arguments = CreateArguments(scriptPath, outputFilePath);
            var result = ExecuteScript(arguments);

            if (!result.Successed)
            {
                throw new Exception(result.Message);
            }

            return ReadOutputData(outputFilePath);
        }

        protected virtual string GetCommandScript(InputData inputData, T configuration)
        {
            if (!configuration.HasGroups)
            {
                return GetSimpleCommandScript(configuration);
            }

            var groups = configuration.GetGroups();
            var builder = new StringBuilder();

            foreach (var group in groups)
            {
                builder.Append(GetGroupCommandScript(group));
                builder.AppendLine();
            }

            return builder.ToString();
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
            return new StatisticProcessExecuter(arguments).Execute();
        }

        protected string CreateArguments(string inputFilePath, string outputFilePath)
        {
            return string.Format(DefaultArgumentsFormat, inputFilePath, outputFilePath);
        }

        protected OutputData ReadOutputData(string filePath)
        {
            return new OutputDataReader().Read(filePath);
        }

        protected abstract string GetSimpleCommandScript(T configuration);

        protected abstract string GetGroupCommandScript(GroupDescription @group);

        protected virtual string GetOptionalEndScript(T configuration)
        {
            return string.Empty;
        }

        private string GetFullCommand(string inputFilePath, InputData inputData)
        {
            return GetConfigurationCommand(inputFilePath, inputData.Configuration) + Environment.NewLine +
                   GetCommandScript(inputData, (T)inputData.Configuration);
        }

        private string GetConfigurationCommand(string inputFilePath, IConfiguration configuration)
        {
            return string.Format(
                CommandConstants.ConfigurationFormat,
                inputFilePath,
                configuration.AllVariables.JoinByNewLine(v => v.GetVariableDefinition()));
        }
    }
}
