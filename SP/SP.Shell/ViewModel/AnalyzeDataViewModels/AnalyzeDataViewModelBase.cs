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

        protected AnalyzeDataViewModelBase(RecordsCollection records, AnalyzeType type)
        {
            Records = records;
            SelectedType = type;
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

        public RelayCommand AnalyzeCommand { get; private set; }

        protected abstract void AnalyzeDataExecute();
    }
}
