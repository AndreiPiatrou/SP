using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;

using SP.Extensions;
using SP.Shell.Models;

namespace SP.Shell.ViewModel
{
    public class TabViewModel : ViewModelBase
    {
        private string title;
        private RecordsCollection records;
        private List<List<string>> sourceCollection;

        public TabViewModel(string title)
        {
            Title = title;
            Records = new RecordsCollection();
            sourceCollection = new List<List<string>>();
        }

        public TabViewModel(string title, List<List<string>> list)
        {
            Title = title;
            Records = new RecordsCollection(list);
            sourceCollection = list;
        }

        public string Title
        {
            get
            {
                return title;
            }

            private set
            {
                title = value.ToUpper();
                RaisePropertyChanged(() => Title);
            }
        }

        public RecordsCollection Records
        {
            get
            {
                return records;
            }

            private set
            {
                records = value;
                RaisePropertyChanged(() => Records);
            }
        }

        public void ResetToSource()
        {
            Records = new RecordsCollection(sourceCollection);
            Records.UpdateRowsAndHeaders();
        }

        public void LoadRecords(IEnumerable<IEnumerable<string>> newRecords, string fileName)
        {
            var enumerable = newRecords as IList<IEnumerable<string>> ?? newRecords.ToList();

            Title = fileName;
            Records = new RecordsCollection(enumerable.Select(i => i.ToList()).ToList());
            sourceCollection = enumerable.ToCompleteList();
        }
    }
}
