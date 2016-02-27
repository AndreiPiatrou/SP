using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Messages;
using SP.Shell.Services;
using SP.Shell.ViewModel;

namespace SP.Shell.Commands
{
    public class FileCommands
    {
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static ICommand OpenFileCommand = new RelayCommand(OpenFileCommandExecute);

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static ICommand SaveToFileCommand = new RelayCommand(SaveToFileCommandExecute);

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
            var readService = GetDataReadService();
            var newTab = false;

            foreach (var worksheet in readService.ReadWorksheets(filePath))
            {
                var tab = GetTab(newTab);
                tab.LoadRecords(worksheet, fileName);
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

        private static IMessenger GetMessenger()
        {
            return ServiceLocator.Current.GetInstance<Messenger>();
        }

        private static TabViewModel GetTab(bool newTab)
        {
            var mainModel = ServiceLocator.Current.GetInstance<MainViewModel>();

            if (!newTab)
            {
                return mainModel.SelectedTab;
            }

            var newTabModel = mainModel.AddNewTabCommand();

            mainModel.Tabs.Add(newTabModel);
            mainModel.SelectedTab = newTabModel;

            return newTabModel;
        }
    }
}
