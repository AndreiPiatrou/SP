using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Messages;
using SP.Shell.Models;

namespace SP.Shell.ViewModel
{
    public class CriteriaRangeViewModel : ViewModelBase
    {
        private RecordsCollection selectedCollection;

        private bool forceHide;

        public CriteriaRangeViewModel()
        {
            MessengerInstance = ServiceLocator.Current.GetInstance<Messenger>();
            HideCommand = new RelayCommand(
                () =>
                    {
                        forceHide = true;
                        RaisePropertyChanged(() => IsVisible);
                    });

            MessengerInstance.Register<DataGridSelectionChangedMessage>(this, SelectionChanged);
        }

        public bool IsVisible
        {
            get { return !forceHide && selectedCollection != null && selectedCollection.SelectedHeader > -1; }
        }

        public ICommand HideCommand { get; private set; }

        private void SelectionChanged(DataGridSelectionChangedMessage message)
        {
            selectedCollection = message.Records;
            forceHide = false;

            RaisePropertyChanged(() => IsVisible);
        }
    }
}
