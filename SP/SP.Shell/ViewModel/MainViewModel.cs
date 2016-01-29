using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

using SP.Shell.Messages;

namespace SP.Shell.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private TabViewModel selectedTab;

        public MainViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>
                       {
                           new TabViewModel("TAB 1"),
                           new TabViewModel("TAB 2")
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