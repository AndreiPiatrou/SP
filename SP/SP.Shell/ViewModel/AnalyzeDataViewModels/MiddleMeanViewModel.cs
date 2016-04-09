using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using SP.Extensions;
using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;
using SP.Resources;
using SP.Shell.Messages;
using SP.Shell.Models;

namespace SP.Shell.ViewModel.AnalyzeDataViewModels
{
    public class MiddleMeanViewModel : AnalyzeDataViewModelBase
    {
        public MiddleMeanViewModel(RecordsCollection records)
            : base(records, AnalyzeType.MiddleMean, Strings.MiddleMean)
        {
            Headers = ExtractHeaders().ToObservable();
        }

        public ObservableCollection<CheckableHeaderModel> Headers { get; private set; }

        public string SelectedCriteria { get; set; }

        protected override void AnalyzeDataExecute()
        {
            MessengerInstance.Send(new AnalyzeDataMessage(ExtractInputData(), SelectedType));
        }

        private IEnumerable<CheckableHeaderModel> ExtractHeaders()
        {
            for (var i = 0; i < Records.Headers.Count - 1; i++)
            {
                yield return new CheckableHeaderModel(Records.Headers[i], i);
            }
        }

        private InputData ExtractInputData()
        {
            throw new NotImplementedException();
            //var checkedHeaders = Headers.Where(h => h.IsChecked).ToList();
            //var indexes = checkedHeaders.Select(h => h.Index);

            //return new InputData
            //           {
            //               Configuration = new MiddleMeanConfiguration
            //                                   {
            //                                       GroupVariables = checkedHeaders.Select(h => h.Header)
            //                                   },
            //               Rows = Records.Records.SkipLast().Select(list => list.Where((r, i) => indexes.Contains(i)))
            //           };
        }
    }
}