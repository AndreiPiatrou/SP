﻿using System;

using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Constants;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Commands.Implementations
{
    public class CorrelationCommand : AnalyzeCommandBase<BivariateCorrelationConfiguration>
    {
        public CorrelationCommand(WorkingDirectory directory) : base(directory)
        {
        }

        protected override string GetScript(InputData inputData, BivariateCorrelationConfiguration configuration, string inputFilePath)
        {
            return string.Format(
                CommandConstants.CorrelationCommandFormat,
                inputFilePath,
                string.Join(" F4" + Environment.NewLine, configuration.Variables),
                string.Join(Environment.NewLine, configuration.Variables));
        }
    }
}
