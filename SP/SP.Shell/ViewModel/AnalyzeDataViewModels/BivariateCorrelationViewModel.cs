using System;

using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;
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

        protected override InputData ExtractInputData()
        {
            throw new NotImplementedException();
        }
    }
}
