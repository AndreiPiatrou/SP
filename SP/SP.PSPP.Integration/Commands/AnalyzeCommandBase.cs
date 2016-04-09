using System;
using System.Collections.Generic;
using System.Linq;
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

        protected abstract string GetCommandScript(InputData inputData, T configuration);

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
        
        protected IEnumerable<string> GetFilterStrings(IList<VariableDescription> descriptions, InputData data, int skipIndex)
        {
            var names = descriptions.Select(d => d.Name).ToList();
            var groups = data.Rows.Select(r => r.Where((e, i) => i != skipIndex).ToList()).Distinct(new EnumerableComparer()).ToList();

            return groups.Select((g, i) => GetGroupFilter(g.ToList(), names));
        }

        private string GetGroupFilter(IList<string> group, IList<string> names)
        {
            var b = new StringBuilder();
            for (var i = 0; i < group.Count; i++)
            {
                var value = group[i].IsNumber() ? group[i] : "'" + group[i] + "'";

                b.Append("( " + names[i] + " = " + value + " ) AND ");
            }

            return b.ToString().TrimEnd(" AND ".ToCharArray());
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
