using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Extensions;
using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;
using SP.Shell.Messages;
using SP.Shell.Models;

namespace SP.Shell.ViewModel
{
    public class AnalyzeDataViewModel : ViewModelBase
    {
        private readonly RecordsCollection records;

        private bool closeRequested;

        public AnalyzeDataViewModel(RecordsCollection records)
        {
            this.records = records;
            MessengerInstance = ServiceLocator.Current.GetInstance<Messenger>();

            Headers = ExtractHeaders().ToObservable();
            Types = new ObservableCollection<AnalyzeType> { AnalyzeType.Correlation };
            AnalyzeCommand = new RelayCommand(
                () =>
                    {
                        MessengerInstance.Send(new AnalyzeDataMessage(ExtractInputData(), SelectedType));
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

        public ObservableCollection<CheckableHeaderModel> Headers { get; private set; }

        public ObservableCollection<AnalyzeType> Types { get; private set; }

        public AnalyzeType SelectedType { get; set; }

        public RelayCommand AnalyzeCommand { get; private set; }

        private IEnumerable<CheckableHeaderModel> ExtractHeaders()
        {
            for (var i = 0; i < records.Headers.Count - 1; i++)
            {
                yield return new CheckableHeaderModel(records.Headers[i], i);
            }
        }

        private InputData ExtractInputData()
        {
            // TODO: update to fill correct info.
            return new InputData
            {
                Variables = records.Headers,
                Rows = records.Records
            };
        }
    }
}
