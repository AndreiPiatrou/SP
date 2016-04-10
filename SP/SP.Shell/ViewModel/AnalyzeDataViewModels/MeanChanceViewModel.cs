using System.Collections.Generic;
using System.Linq;

using SP.Extensions;
using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;
using SP.PSPP.Integration.Models.Configuration;
using SP.Resources;
using SP.Shell.Models;

namespace SP.Shell.ViewModel.AnalyzeDataViewModels
{
    public class MeanChanceViewModel : AnalyzeDataViewModelBase
    {
        public MeanChanceViewModel(RecordsCollection records)
            : base(records, AnalyzeType.MeanChance, Strings.MeanChance)
        {
        }

        protected override InputData ExtractInputData()
        {
            var criteria = Criteria.First(c => c.IsChecked);
            var checkedHeaders = GroupHeaders.Where(h => h.IsChecked).ToList();
            var indexes = checkedHeaders.Select(h => h.Index).ToList();
            var allRows = Records.Records.SkipLast().Select(list => list.Where((r, i) => indexes.Contains(i) || i == criteria.Index)).ToList();

            var groupVariables = GetGroupVariables(checkedHeaders);
            var targetVariable = GetTargetVariable(criteria);

            return new InputData(allRows, new MeanChanceConfiguration(groupVariables, targetVariable));
        }

        private IEnumerable<VariableDescription> GetGroupVariables(List<CheckableHeaderModel> checkedHeaders)
        {
            return checkedHeaders.Select(
                h =>
                new VariableDescription(
                    h.Index,
                    h.Header,
                    Records.Records.Select(e => e.Where((c, i) => h.Index == i)).First().IsNumberOrEmptyString()));
        }

        private VariableDescription GetTargetVariable(CheckableHeaderModel criteria)
        {
            return new VariableDescription(
                criteria.Index,
                criteria.Header,
                Records.Records.Select(e => e.Where((c, i) => criteria.Index == i)).First().IsNumberOrEmptyString());
        }
    }
}
