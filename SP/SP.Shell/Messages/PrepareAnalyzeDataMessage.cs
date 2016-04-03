using System.Collections.ObjectModel;

using SP.Shell.Models;
using SP.Shell.ViewModel.AnalyzeDataViewModels;

namespace SP.Shell.Messages
{
    public class PrepareAnalyzeDataMessage
    {
        public PrepareAnalyzeDataMessage(AnalyzeDataViewModelBase model)
        {
            Model = model;
        }

        public ObservableCollection<CheckableHeaderModel> Headers { get; private set; }

        public AnalyzeDataViewModelBase Model { get; private set; }
    }
}
