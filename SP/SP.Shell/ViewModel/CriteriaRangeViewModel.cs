using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Extensions;
using SP.Shell.Messages;
using SP.Shell.Models;

namespace SP.Shell.ViewModel
{
    public class CriteriaRangeViewModel : ViewModelBase
    {
        private RecordsCollection selectedCollection;

        private bool forceHide;
        private bool hasChanges;

        public CriteriaRangeViewModel()
        {
            MessengerInstance = ServiceLocator.Current.GetInstance<Messenger>();
            HideCommand = new RelayCommand(HideCommandExecute);
            ApplyCommand = new RelayCommand(ApplyCommandExecute, ApplyCommandCanExecute);

            MessengerInstance.Register<DataGridSelectionChangedMessage>(this, SelectionChanged);
        }

        public bool IsVisible
        {
            get
            {
                return (!forceHide && selectedCollection != null && selectedCollection.SelectedHeader > -1) ||
                       CurrentSelector == null;
            }
        }

        public bool IsNumericRangeSelected
        {
            get { return IsVisible && SelectedCriteria.All(i => i.IsNumberOrEmpty()); }
        }

        public IEnumerable<string> SelectedCriteria
        {
            get
            {
                return IsVisible
                           ? selectedCollection.Records.Select(s => s.ElementAt(selectedCollection.SelectedHeader))
                           : Enumerable.Empty<string>();
            }
        }

        public ICommand HideCommand { get; private set; }

        public RelayCommand ApplyCommand { get; private set; }

        public ICriteriaRangeSelector CurrentSelector { get; private set; }

        private void SelectionChanged(DataGridSelectionChangedMessage message)
        {
            selectedCollection = message.Records;
            forceHide = false;
            hasChanges = false;

            CurrentSelector = GetSelector();
            ApplyCommand.RaiseCanExecuteChanged();

            RaisePropertyChanged(() => CurrentSelector);
            RaisePropertyChanged(() => IsVisible);
        }

        private void SelectedOnChange()
        {
            hasChanges = true;
            ApplyCommand.RaiseCanExecuteChanged();
        }

        private void ApplyCommandExecute()
        {
            CurrentSelector.Apply();
            SelectionChanged(new DataGridSelectionChangedMessage(selectedCollection));
        }

        private bool ApplyCommandCanExecute()
        {
            return hasChanges && CurrentSelector.CanApply();
        }

        private void HideCommandExecute()
        {
            forceHide = true;
            RaisePropertyChanged(() => IsVisible);
        }

        private ICriteriaRangeSelector GetSelector()
        {
            return IsNumericRangeSelected
                       ? (ICriteriaRangeSelector)
                         new NumericCriteriaRangeViewModel(
                             SelectedOnChange,
                             () => SelectedCriteria,
                             () => selectedCollection)
                       : new DiscreteCriteriaRangeViewModel(
                             SelectedOnChange,
                             () => SelectedCriteria,
                             () => selectedCollection);
        }
    }
}
