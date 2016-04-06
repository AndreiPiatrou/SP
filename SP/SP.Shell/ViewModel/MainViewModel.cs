using System.Collections.ObjectModel;

using Dragablz;

using GalaSoft.MvvmLight;

using SP.Resources;
using SP.Shell.Behaviors;

namespace SP.Shell.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private TabViewModel selectedTab;

        public MainViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>
                       {
                           new TabViewModel(Strings.NewTab)
                       };
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

        public void AddAndSelectTab(TabViewModel tab)
        {
            Tabs.Add(tab);
            SelectedTab = tab;
        }

        public IInterTabClient Client
        {
            get { return new NoNewWindowInterTabClient(); }
        }
    }
}