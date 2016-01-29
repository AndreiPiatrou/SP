using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using SP.FIleSystem;

namespace SP.PSPP.Integration
{
    public class StatisticProcessRunner
    {
        private const string RelativeExecutablePath = "\\Executable\\pspp.exe";

        private readonly IList<string> output;
        private readonly Process statisticProcess;

        public StatisticProcessRunner(string arguments)
        {
            output = new List<string>();

            statisticProcess = CreateProcess(arguments);
        }

        public StatisticProcessRunResult Start()
        {
            try
            {
                statisticProcess.Start();
                statisticProcess.WaitForExit();

                return new StatisticProcessRunResult(true);
            }
            catch (Exception ex)
            {
                return output.Any()
                           ? new StatisticProcessRunResult(string.Join(Environment.NewLine, output))
                           : new StatisticProcessRunResult(ex.ToString());
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
                EnableRaisingEvents = true
            };

            process.ErrorDataReceived += ProcessOnErrorDataReceived;

            return process;
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            output.Add(e.Data);
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
