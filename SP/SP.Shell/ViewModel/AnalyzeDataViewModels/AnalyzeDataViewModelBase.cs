using System;
using System.Collections.Generic;

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
                    },
                AnalyzeDataCanExecute);
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
        
        protected IEnumerable<CheckableHeaderModel> ExtractHeaders()
        {
            for (var i = 0; i < Records.Headers.Count - 1; i++)
            {
                yield return new CheckableHeaderModel(Records.Headers[i], i, AnalyzeCommand.RaiseCanExecuteChanged);
            }
        }

        protected abstract void AnalyzeDataExecute();

        protected abstract bool AnalyzeDataCanExecute();
    }
}
