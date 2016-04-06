using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.PSPP.Integration.Commands;
using SP.Shell.Factories;
using SP.Shell.Messages;
using SP.Shell.Models;
using SP.Shell.ViewModel;

namespace SP.Shell.Commands
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
    public class AnalyzeCommands
    {
        public static ICommand AnalyzeDataCommand = new RelayCommand<AnalyzeType>(AnalyzeDataCommandExecute);

        private static void AnalyzeDataCommandExecute(AnalyzeType analyzeType)
        {
            var messenger = GetMessenger();
            var currentRecords = GetCurrentRecordsCollection();
            var model = AnalyzeDataViewModelFactory.GetModel(analyzeType, currentRecords);

            messenger.Send(new PrepareAnalyzeDataMessage(model));
        }

        private static RecordsCollection GetCurrentRecordsCollection()
        {
            return ServiceLocator.Current.GetInstance<MainViewModel>().SelectedTab.Records;
        }

        private static IMessenger GetMessenger()
        {
            return ServiceLocator.Current.GetInstance<Messenger>();
        }
    }
}
