using System;
using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Messages;
using SP.Shell.Services;

namespace SP.Shell.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private TabViewModel selectedTab;

        public MainViewModel()
        {
            MessengerInstance = ServiceLocator.Current.GetInstance<Messenger>();
            Tabs = new ObservableCollection<TabViewModel>
                       {
                           new TabViewModel("NEW TAB"),
                       };
            MessengerInstance.Register<AnalyzeDataMessage>(this, AnalyzeDataExecute);
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

        public AnalysisService Service
        {
            get { return ServiceLocator.Current.GetInstance<AnalysisService>(); }
        }

        private void AnalyzeDataExecute(AnalyzeDataMessage message)
        {
            var result = Service.Analyze(message.InputData, message.Type);
            var newTab = new TabViewModel("RESULT", result.Rows.Select(i => i.ToList()).ToList());

            Tabs.Add(newTab);
            SelectedTab = newTab;
        }
    }
}