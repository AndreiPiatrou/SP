using SP.FIleSystem.Directory;
using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;

namespace SP.Shell.Services
{
    public class AnalysisService
    {
        private readonly WorkingDirectory directory;

        public AnalysisService(WorkingDirectory directory)
        {
            this.directory = directory;
        }

        public OutputData Analyze(InputData inputData, AnalyzeType analyzeType)
        {
            return AnalyzeCommandFactory.CreateCommand(analyzeType, directory).Analyze(inputData);
        }
    }
}
