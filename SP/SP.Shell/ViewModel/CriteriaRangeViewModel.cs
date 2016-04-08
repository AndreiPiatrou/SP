using System;
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

        public CriteriaRangeViewModel()
        {
            MessengerInstance = ServiceLocator.Current.GetInstance<Messenger>();
            HideCommand = new RelayCommand(
                () =>
                    {
                        forceHide = true;
                        RaisePropertyChanged(() => IsVisible);
                    });
            ApplyCommand = new RelayCommand(ApplyCommandExecute);

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

        public double SelectedMin { get; set; }

        public double SelectedMax { get; set; }

        public IEnumerable<SelectableValue> Values { get; private set; }

        public ICommand HideCommand { get; private set; }

        public ICommand ApplyCommand { get; private set; }

        public double Frequency
        {
            get { return (Max - Min) / 20; }
        }

        private void SelectionChanged(DataGridSelectionChangedMessage message)
        {
            selectedCollection = message.Records;
            forceHide = false;

            RaisePropertyChanged(() => IsVisible);
            RaisePropertyChanged(() => IsNumericRangeSelected);

            if (IsNumericRangeSelected)
            {
                var numbers = SelectedCriteria.Where(v => v.IsNumber()).Select(v => v.ToNumber()).ToList();

                if (!numbers.Any())
                {
                    Min = 0;
                    Max = 0;
                    SelectedMin = 0;
                    SelectedMax = 0;
                }
                else
                {
                    Min = numbers.Min();
                    Max = numbers.Max();

                    SelectedMin = Min;
                    SelectedMax = Max;
                }

                RaisePropertyChanged(() => Min);
                RaisePropertyChanged(() => Max);
                RaisePropertyChanged(() => SelectedMin);
                RaisePropertyChanged(() => SelectedMax);
            }
            else
            {
                Values = SelectedCriteria.Select(v => new SelectableValue(v, true));

                RaisePropertyChanged(() => Values);
            }
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
    }
}
