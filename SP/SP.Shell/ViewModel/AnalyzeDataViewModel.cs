using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using SP.Extensions;
using SP.Shell.Models;

namespace SP.Shell.ViewModel
{
    public class AnalyzeDataViewModel : ViewModelBase
    {
        private readonly RecordsCollection records;

        public AnalyzeDataViewModel(RecordsCollection records)
        {
            this.records = records;

            Headers = ExtractHeaders().ToObservable();
        }

        public ObservableCollection<CheckableHeaderModel> Headers { get; private set; }

        private IEnumerable<CheckableHeaderModel> ExtractHeaders()
        {
            for (var i = 0; i < records.Headers.Count - 1; i++)
            {
                yield return new CheckableHeaderModel(records.Headers[i], i);
            }
        }
    }
}
