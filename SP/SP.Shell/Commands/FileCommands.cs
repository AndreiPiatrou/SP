using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Resources;
using SP.Shell.Messages;
using SP.Shell.Services;
using SP.Shell.Tasks;
using SP.Shell.ViewModel;

namespace SP.Shell.Commands
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
    public class FileCommands
    {
        public static ICommand OpenFileCommand = new RelayCommand(OpenFileCommandExecute);

        public static ICommand SaveToFileCommand = new RelayCommand(SaveToFileCommandExecute);

        public static Func<TabViewModel> CreateNewTabCommand
        {
            get
            {
                return () => new TabViewModel(GetTabNameService().GetImportedData(GetCurrentTabNames()));
            }
        }

        private static void OpenFileCommandExecute()
        {
            var messenger = GetMessenger();

            messenger.Send(new OpenFileMessage(OpenFilePositiveCallback));
        }

        private static void SaveToFileCommandExecute()
        {
            var messenger = GetMessenger();
            var records = GetCurrentTab().Records;

            messenger.Send(new AskForFilePathMessage(s => messenger.Send(new SaveRecordsToFileMessage(records, s))));
        }

        private static void OpenFilePositiveCallback(string filePath, string fileName)
        {
            TaskService.WrapToTask(() => ReadDataFromfile(filePath), data => ApplyDataFromfile(data, fileName));
        }

        private static IEnumerable<IEnumerable<IEnumerable<string>>> ReadDataFromfile(string filePath)
        {
            return GetDataReadService().ReadWorksheets(filePath).ToList();
        }

        private static void ApplyDataFromfile(IEnumerable<IEnumerable<IEnumerable<string>>> data, string fileName)
        {
            var newTab = false;
            var tabNameService = GetTabNameService();

            foreach (var worksheet in data)
            {
                var tab = GetTab(newTab);
                tab.LoadRecords(worksheet, tabNameService.GetImportedData(GetCurrentTabNames(), fileName));
                newTab = true;
            }
        }

        private static TabViewModel GetCurrentTab()
        {
            return ServiceLocator.Current.GetInstance<MainViewModel>().SelectedTab;
        }

        private static DataReadService GetDataReadService()
        {
            return ServiceLocator.Current.GetInstance<DataReadService>();
        }

        private static TabNameService GetTabNameService()
        {
            return ServiceLocator.Current.GetInstance<TabNameService>();
        }

        private static IMessenger GetMessenger()
        {
            return ServiceLocator.Current.GetInstance<Messenger>();
        }

        private static TabViewModel GetTab(bool newTab)
        {
            var mainModel = ServiceLocator.Current.GetInstance<MainViewModel>();

            if (!newTab && mainModel.SelectedTab != null)
            {
                return mainModel.SelectedTab;
            }

            var newTabModel = CreateNewTabCommand();

            mainModel.Tabs.Add(newTabModel);
            mainModel.SelectedTab = newTabModel;

            return newTabModel;
        }

        private static IEnumerable<string> GetCurrentTabNames()
        {
            return ServiceLocator.Current.GetInstance<MainViewModel>().Tabs.Select(t => t.Title);
        }
    }
}
