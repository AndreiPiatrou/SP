using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using SP.Extensions;
using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;
using SP.Resources;
using SP.Shell.Messages;
using SP.Shell.Models;

namespace SP.Shell.ViewModel.AnalyzeDataViewModels
{
    public class BivariateCorrelationViewModel : AnalyzeDataViewModelBase
    {
        public BivariateCorrelationViewModel(RecordsCollection records)
            : base(records, AnalyzeType.Correlation, Strings.CorrelationCoefficient)
        {
            Headers = ExtractHeaders().ToObservable();
        }

        public ObservableCollection<CheckableHeaderModel> Headers { get; private set; }

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
            var checkedHeaders = Headers.Where(h => h.IsChecked).ToList();
            var indexes = checkedHeaders.Select(h => h.Index);

            return new InputData
                       {
                           Variables = checkedHeaders.Select(h => h.Header),
                           Rows = Records.Records.SkipLast().Select(list => list.Where((r, i) => indexes.Contains(i)))
                       };
        }
    }
}
