using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.PSPP.Integration.Commands;
using SP.Shell.Models;

namespace SP.Shell.ViewModel.AnalyzeDataViewModels
{
    public abstract class AnalyzeDataViewModelBase : ViewModelBase
    {
        protected readonly RecordsCollection Records;

        private bool closeRequested;

        protected AnalyzeDataViewModelBase(RecordsCollection records, AnalyzeType type, string title)
        {
            Records = records;
            SelectedType = type;
            Title = title;
            MessengerInstance = ServiceLocator.Current.GetInstance<Messenger>();
            AnalyzeCommand = new RelayCommand(
                () =>
                    {
                        AnalyzeDataExecute();
                        CloseRequested = true;
                    });
        }

        public bool CloseRequested
        {
            get
            {
                return closeRequested;
            }

            set
            {
                closeRequested = value;
                RaisePropertyChanged(() => CloseRequested);
            }
        }

        public AnalyzeType SelectedType { get; private set; }

        public string Title { get; private set; }

        public RelayCommand AnalyzeCommand { get; private set; }

        protected abstract void AnalyzeDataExecute();
    }
}
