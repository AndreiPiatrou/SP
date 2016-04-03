using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;

namespace SP.Shell.Messages
{
    public class AnalyzeDataMessage
    {
        public AnalyzeDataMessage(InputData inputData, AnalyzeType type)
        {
            InputData = inputData;
            Type = type;
        }

        public InputData InputData { get; private set; }

        public AnalyzeType Type { get; private set; }
    }
}
