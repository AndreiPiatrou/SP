using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Models;
using SP.Shell.Services;

namespace SP.Shell.ViewModel
{
    public class TabViewModel : ViewModelBase
    {
        private string title;
        private RecordsCollection records;

        public TabViewModel(string title)
        {
            Title = title;
            Records = new RecordsCollection();
        }

        public TabViewModel(string title, List<List<string>> list)
        {
            Title = title;
            Records = new RecordsCollection(list);
        }

        public DataReadService DataReadService
        {
            get { return ServiceLocator.Current.GetInstance<DataReadService>(); }
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

        public void LoadRecords(IEnumerable<IEnumerable<string>> newRecords, string fileName)
        {
            Title = fileName;
            Records = new RecordsCollection(newRecords.Select(i => i.ToList()).ToList());
        }
    }
}
