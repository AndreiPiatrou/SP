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
        private double selectedMin;
        private double selectedMax;

        public CriteriaRangeViewModel()
        {
            MessengerInstance = ServiceLocator.Current.GetInstance<Messenger>();
            HideCommand = new RelayCommand(
                () =>
                    {
                        forceHide = true;
                        RaisePropertyChanged(() => IsVisible);
                    });
            ApplyCommand = new RelayCommand(ApplyCommandExecute, ApplyCommandCanExecute);

            MessengerInstance.Register<DataGridSelectionChangedMessage>(this, SelectionChanged);
        }

        public bool IsVisible
        {
            get { return !forceHide && selectedCollection != null && selectedCollection.SelectedHeader > -1; }
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

        public double Min { get; set; }

        public double Max { get; set; }

        public double SelectedMin
        {
            get
            {
                return selectedMin;
            }

            set
            {
                selectedMin = value;
                hasChanges = true;
            }
        }

        public double SelectedMax
        {
            get
            {
                return selectedMax;
            }

            set
            {
                selectedMax = value;
                hasChanges = true;
            }
        }

        public IEnumerable<SelectableValue> Values { get; private set; }

        public ICommand HideCommand { get; private set; }

        public RelayCommand ApplyCommand { get; private set; }

        public double Frequency
        {
            get { return (Max - Min) / 20d; }
        }

        public string SelectedValues
        {
            get
            {
                return Values == null
                           ? string.Empty
                           : string.Join(", ", Values.Where(v => v.Selected).Select(v => v.Value));
            }
        }

        private void SelectionChanged(DataGridSelectionChangedMessage message)
        {
            selectedCollection = message.Records;
            forceHide = false;
            hasChanges = false;

            RaisePropertyChanged(() => IsVisible);
            RaisePropertyChanged(() => IsNumericRangeSelected);

            if (IsNumericRangeSelected)
            {
                UpdateNumericProperties();
            }
            else
            {
                UpdateConcreteProperties();
            }

            ApplyCommand.RaiseCanExecuteChanged();
        }

        private void UpdateNumericProperties()
        {
            var numbers = SelectedCriteria.Where(v => v.IsNumber()).Select(v => v.ToNumber()).ToList();

            if (!numbers.Any())
            {
                Min = 0;
                Max = 0;
                selectedMin = 0;
                selectedMax = 0;
            }
            else
            {
                Min = numbers.Min();
                Max = numbers.Max();

                selectedMin = Min;
                selectedMax = Max;
            }

            RaisePropertyChanged(() => Min);
            RaisePropertyChanged(() => Max);
            RaisePropertyChanged(() => SelectedMin);
            RaisePropertyChanged(() => SelectedMax);
            RaisePropertyChanged(() => Frequency);
        }

        private void UpdateConcreteProperties()
        {
            Values =
                SelectedCriteria.Distinct()
                    .Where(v => !string.IsNullOrEmpty(v))
                    .Select(v => new SelectableValue(v, true, SelectedOnChange))
                    .ToList();

            RaisePropertyChanged(() => Values);
            RaisePropertyChanged(() => SelectedValues);
        }

        private void SelectedOnChange()
        {
            hasChanges = true;

            RaisePropertyChanged(() => SelectedValues);
            ApplyCommand.RaiseCanExecuteChanged();
        }

        private void ApplyCommandExecute()
        {
            if (IsNumericRangeSelected)
            {
                selectedCollection.Apply(SelectedMin, SelectedMax);
            }
            else
            {
                selectedCollection.Apply(Values.Where(v => v.Selected).Select(v => v.Value).ToList());
            }

            SelectionChanged(new DataGridSelectionChangedMessage(selectedCollection));
        }

        private bool ApplyCommandCanExecute()
        {
            return hasChanges && (IsNumericRangeSelected || !string.IsNullOrEmpty(SelectedValues));
        }
    }
}
