using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace SP.Shell.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private TabViewModel selectedTab;

        public MainViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>
                       {
                           new TabViewModel("NEW TAB"),
                       };
        }

        public IMessenger Messenger
        {
            get { return SimpleIoc.Default.GetInstance<Messenger>(); }
        }

        public ObservableCollection<TabViewModel> Tabs { get; private set; }

        public TabViewModel SelectedTab
        {
            get
            {
                return selectedTab;
            }

            set
            {
                selectedTab = value;
                RaisePropertyChanged(() => SelectedTab);
            }
        }

        public Func<TabViewModel> AddNewTabCommand
        {
            get
            {
                return () => new TabViewModel("NEW TAB");
            }
        }
    }
}