using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using SP.FIleSystem;

namespace SP.PSPP.Integration
{
    public class StatisticProcessExecuter
    {
        private const string RelativeExecutablePath = "\\Executable\\pspp.exe";

        private readonly IList<string> output;
        private readonly Process statisticProcess;

        public StatisticProcessExecuter(string arguments)
        {
            output = new List<string>();

            statisticProcess = CreateProcess(arguments);
        }

        public StatisticProcessExecutionResult Execute()
        {
            try
            {
                statisticProcess.Start();
                statisticProcess.BeginOutputReadLine();
                statisticProcess.BeginErrorReadLine();
                statisticProcess.WaitForExit();

                return output.Any()
                           ? new StatisticProcessExecutionResult(string.Join(Environment.NewLine, output))
                           : new StatisticProcessExecutionResult(true);
            }
            catch (Exception ex)
            {
                return output.Any()
                           ? new StatisticProcessExecutionResult(string.Join(Environment.NewLine, output))
                           : new StatisticProcessExecutionResult(ex.ToString());
            }
            finally
            {
                statisticProcess.Dispose();
            }
        }

        private Process CreateProcess(string arguments)
        {
            var process = new Process
            {
                StartInfo = CreateProcessStartInfo(arguments),
                EnableRaisingEvents = true,
            };

            process.ErrorDataReceived += ProcessOnErrorDataReceived;
            process.OutputDataReceived += ProcessOnErrorDataReceived;

            return process;
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                output.Add(e.Data);
            }
        }

        private ProcessStartInfo CreateProcessStartInfo(string arguments)
        {
            var processStartInfo = new ProcessStartInfo(PathOperations.AppPathCombine(RelativeExecutablePath))
            {
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            return processStartInfo;
        }
    }
}
