using System;

using SP.PSPP.Integration.Commands;
using SP.PSPP.Integration.Models;
using SP.Resources;
using SP.Shell.Models;

namespace SP.Shell.ViewModel.AnalyzeDataViewModels
{
    public class MiddleMeanViewModel : AnalyzeDataViewModelBase
    {
        public MiddleMeanViewModel(RecordsCollection records)
            : base(records, AnalyzeType.MiddleMean, Strings.MiddleMean)
        {
        }

        protected override InputData ExtractInputData()
        {
            throw new NotImplementedException();
        }
    }
}