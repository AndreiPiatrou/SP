using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Extensions;
using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;
using SP.Shell.Messages;
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

            GroupHeaders = ExtractHeaders().ToObservable();
            Criteria = ExtractHeaders().ToObservable();
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
        
        public ObservableCollection<CheckableHeaderModel> GroupHeaders { get; protected set; }

        public ObservableCollection<CheckableHeaderModel> Criteria { get; protected set; }

        public AnalyzeType SelectedType { get; private set; }

        public string Title { get; protected set; }

        public RelayCommand AnalyzeCommand { get; protected set; }
        
        protected IEnumerable<CheckableHeaderModel> ExtractHeaders()
        {
            for (var i = 0; i < Records.Headers.Count - 1; i++)
            {
                var localIndex = i;
                yield return new CheckableHeaderModel(Records.Headers[i], i, AnalyzeCommand.RaiseCanExecuteChanged)
                                 {
                                     Values = Records.Records.Select(r => r[localIndex])
                                 };
            }
        }

        protected virtual void AnalyzeDataExecute()
        {
            MessengerInstance.Send(new AnalyzeDataMessage(ExtractInputData(), SelectedType));
        }

        protected virtual bool AnalyzeDataCanExecute()
        {
            var selectedCriteria = Criteria.Where(c => c.IsChecked).Select(c => c.Index).ToList();
            var selectedHeaders = GroupHeaders.Where(gh => gh.IsChecked).Select(gh => gh.Index).ToList();

            return selectedCriteria.Any() && selectedHeaders.All(h => !selectedCriteria.Contains(h));
        }

        protected abstract InputData ExtractInputData();
    }
}
