using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

using SP.Shell.Messages;
using SP.Shell.Services;

namespace SP.Shell.ViewModel
{
    public class TabViewModel : ViewModelBase
    {
        private string title;

        public TabViewModel(string title)
        {
            Title = title;

            Records = new ObservableCollection<ObservableCollection<string>>();
            OpenFileCommand = new RelayCommand(() => Messenger.Send(new OpenFileMessage(ReadOpenedFile)));
        }

        public Messenger Messenger
        {
            get { return SimpleIoc.Default.GetInstance<Messenger>(); }
        }

        public DataReadService DataReadService
        {
            get { return SimpleIoc.Default.GetInstance<DataReadService>(); }
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

        public ObservableCollection<ObservableCollection<string>> Records { get; private set; }

        public RelayCommand OpenFileCommand { get; private set; }

        private void ReadOpenedFile(string path, string fileName)
        {
            Title = fileName;

            foreach (var columns in DataReadService.ReadFile(path))
            {
                Records.Add(new ObservableCollection<string>(columns));
            }
        }
    }
}
