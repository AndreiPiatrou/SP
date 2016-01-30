using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Messages;
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
            MessengerInstance = ServiceLocator.Current.GetInstance<Messenger>();
            Title = title;
            Records = new RecordsCollection();

            InitializeCommands();
        }

        public TabViewModel(string title, List<List<string>> list)
        {
            Title = title;
            Records = new RecordsCollection(list);

            InitializeCommands();
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

        public RelayCommand OpenFileCommand { get; private set; }

        private void InitializeCommands()
        {
            OpenFileCommand = new RelayCommand(() => MessengerInstance.Send(new OpenFileMessage(ReadOpenedFile)));
        }

        private void ReadOpenedFile(string path, string fileName)
        {
            var data = DataReadService.ReadFile(path).Select(i => i.ToList()).ToList();

            Title = fileName;
            Records = new RecordsCollection(data);
        }
    }
}
