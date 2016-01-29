using SP.PSPP.Integration.Models;

namespace SP.PSPP.Integration.Commands
{
    public interface IAnalyzeCommand
    {
        OutputData Analyze(InputData inputData);
    }
}
