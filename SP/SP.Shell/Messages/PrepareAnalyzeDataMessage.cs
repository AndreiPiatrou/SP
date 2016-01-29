using SP.Shell.ViewModel;

namespace SP.Shell.Messages
{
    public class PrepareAnalyzeDataMessage
    {
        public PrepareAnalyzeDataMessage(AnalyzeDataViewModel model)
        {
            Model = model;
        }

        public AnalyzeDataViewModel Model { get; private set; }
    }
}
