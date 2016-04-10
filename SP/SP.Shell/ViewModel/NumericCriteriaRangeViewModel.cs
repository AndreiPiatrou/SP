using System;
using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;

using SP.Extensions;
using SP.Shell.Models;

namespace SP.Shell.ViewModel
{
    public class NumericCriteriaRangeViewModel : ViewModelBase, ICriteriaRangeSelector
    {
        private readonly Action rangeChangedAction;
        private readonly Func<IEnumerable<string>> criteriaSelector;
        private readonly Func<RecordsCollection> collectionSelector;

        private double selectedMin;
        private double selectedMax;

        public NumericCriteriaRangeViewModel(
            Action rangeChangedAction,
            Func<IEnumerable<string>> criteriaSelector,
            Func<RecordsCollection> collectionSelector)
        {
            this.rangeChangedAction = rangeChangedAction;
            this.criteriaSelector = criteriaSelector;
            this.collectionSelector = collectionSelector;

            UpdateProperties();
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
                rangeChangedAction();
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
                rangeChangedAction();
            }
        }

        public double Frequency
        {
            get { return Math.Abs(Min - Max) > 0 ? (Max - Min) / 20d : 1; }
        }

        public void Apply()
        {
            collectionSelector().Apply(SelectedMin, selectedMax);
        }

        public bool CanApply()
        {
            return true;
        }

        private void UpdateProperties()
        {
            var numbers = criteriaSelector().Where(v => v.IsNumber()).Select(v => v.ToNumber()).ToList();

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
    }
}