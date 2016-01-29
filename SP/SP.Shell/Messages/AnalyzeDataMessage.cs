using SP.Shell.ViewModel;

namespace SP.Shell.Messages
{
    public class AnalyzeDataMessage
    {
        public AnalyzeDataMessage(AnalyzeDataViewModel model)
        {
            Model = model;
        }

        public AnalyzeDataViewModel Model { get; private set; }
    }
}
