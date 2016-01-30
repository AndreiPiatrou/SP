using System;

using SP.PSPP.Integration.Commands;
using SP.Shell.Models;
using SP.Shell.ViewModel.AnalyzeDataViewModels;

namespace SP.Shell.Factories
{
    public class AnalyzeDataViewModelFactory
    {
        public static AnalyzeDataViewModelBase GetModel(AnalyzeType analyzeType, RecordsCollection records)
        {
            switch (analyzeType)
            {
                case AnalyzeType.Correlation:
                    return new BivariateCorrelationViewModel(records);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
