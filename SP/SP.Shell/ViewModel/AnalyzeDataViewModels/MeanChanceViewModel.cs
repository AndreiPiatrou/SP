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
    public class MeanChanceViewModel : AnalyzeDataViewModelBase
    {
        public MeanChanceViewModel(RecordsCollection records)
            : base(records, AnalyzeType.MeanChance, Strings.MeanChance)
        {
            GroupHeaders = ExtractHeaders().ToObservable();
            Criteria = ExtractHeaders().ToObservable();
        }

        public ObservableCollection<CheckableHeaderModel> GroupHeaders { get; private set; }

        public ObservableCollection<CheckableHeaderModel> Criteria { get; private set; }

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
            var criteria = Criteria.First(c => c.IsChecked);
            var checkedHeaders = GroupHeaders.Where(h => h.IsChecked).ToList();
            var indexes = checkedHeaders.Select(h => h.Index).ToList();
            var allRows = Records.Records.SkipLast().Select(list => list.Where((r, i) => indexes.Contains(i) || i == criteria.Index)).ToList();

            var groupVariables =
                checkedHeaders.Select(
                    h =>
                    new VariableDescription(
                        h.Index,
                        h.Header,
                        Records.Records.Select(e => e.Where((c, i) => h.Index == i)).First().IsNumberOrEmptyString()));
            var targetVariable = new VariableDescription(
                criteria.Index,
                criteria.Header,
                Records.Records.Select(e => e.Where((c, i) => criteria.Index == i)).First().IsNumberOrEmptyString());

            return new InputData(allRows, new MeanChanceConfiguration(groupVariables, targetVariable));
        }
    }
}
