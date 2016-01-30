using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Messages;
using SP.Shell.ViewModel;

namespace SP.Shell.Commands
{
    public class FileCommands
    {
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static ICommand OpenFileCommand = new RelayCommand(OpenFileCommandExecute);

        private static void OpenFileCommandExecute()
        {
            var messenger = GetMessenger();

            messenger.Send(new OpenFileMessage(OpenFilePositiveCallback));
        }

        private static void OpenFilePositiveCallback(string filePath, string fileName)
        {
            var tab = GetCurrentTab();

            tab.LoadFileToRecords(filePath, fileName);
        }

        private static TabViewModel GetCurrentTab()
        {
            return ServiceLocator.Current.GetInstance<MainViewModel>().SelectedTab;
        }

        private static IMessenger GetMessenger()
        {
            return ServiceLocator.Current.GetInstance<Messenger>();
        }
    }
}
