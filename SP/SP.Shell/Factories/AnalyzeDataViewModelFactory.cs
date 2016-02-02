using System;
using System.Collections.Generic;

using SP.PSPP.Integration.Commands;
using SP.Shell.Models;
using SP.Shell.ViewModel.AnalyzeDataViewModels;

namespace SP.Shell.Factories
{
    public class AnalyzeDataViewModelFactory
    {
        private static readonly IDictionary<AnalyzeType, Func<RecordsCollection, AnalyzeDataViewModelBase>> Dictionary = new Dictionary<AnalyzeType, Func<RecordsCollection, AnalyzeDataViewModelBase>>
        {
            { AnalyzeType.Correlation, collection => new BivariateCorrelationViewModel(collection) },
            { AnalyzeType.MiddleMean, collection => new MiddleMeadViewModel(collection) },
        };

        public static AnalyzeDataViewModelBase GetModel(AnalyzeType analyzeType, RecordsCollection records)
        {
            if (!Dictionary.ContainsKey(analyzeType))
            {
                throw new NotImplementedException();
            }

            return Dictionary[analyzeType](records);
        }
    }
}
