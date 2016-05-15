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
    public class PearsonCorrelationViewModel : AnalyzeDataViewModelBase
    {
        public PearsonCorrelationViewModel(RecordsCollection records)
            : base(records, AnalyzeType.PearsonCorrelation, Strings.CorrelationCoefficient)
        {
        }

        protected override bool AnalyzeDataCanExecute()
        {
            return base.AnalyzeDataCanExecute() && Criteria.Count(c => c.IsChecked) == 2 && AllCriteriaAreNumeric();
        }

        protected override InputData ExtractInputData()
        {
            var criteria = Criteria.Where(c => c.IsChecked).ToList();
            var checkedHeaders = GroupHeaders.Where(h => h.IsChecked).ToList();

            var indexes = checkedHeaders.Select(h => h.Index).Concat(criteria.Select(c => c.Index)).ToList();
            var allRows = Records.Records.SkipLast().Select(list => list.Where((r, i) => indexes.Contains(i))).ToList();

            var groupVariables = GetGroupVariables(checkedHeaders);
            var targetVariable = GetCriteriaVariables(criteria);

            return new InputData(allRows, new PearsonCorrelationConfiguration(groupVariables, targetVariable));
        }

        private IEnumerable<VariableDescription> GetGroupVariables(IEnumerable<CheckableHeaderModel> criteria)
        {
            return
                criteria.Select(
                    c =>
                    new VariableDescription(
                        c.Index,
                        c.Header,
                        Records.Records.Select(e => e.Where((cr, i) => c.Index == i)).First().IsNumberOrEmptyString(),
                        Records.Records.Select(e => e.Where((cr, i) => c.Index == i).First()).SkipLast()));
        }

        private IEnumerable<VariableDescription> GetCriteriaVariables(IEnumerable<CheckableHeaderModel> criteria)
        {
            return
                criteria.Select(
                    c =>
                    new VariableDescription(
                        c.Index,
                        c.Header,
                        Records.Records.Select(e => e.Where((cr, i) => c.Index == i)).First().IsNumberOrEmptyString(),
                        Records.Records.Select(e => e.Where((cr, i) => c.Index == i).First()).SkipLast(),
                        c.SelectedValue));
        }

        private bool AllCriteriaAreNumeric()
        {
            return Criteria.Where(c => c.IsChecked).All(c => c.Values.IsNumberOrEmptyString());
        }
    }
}
