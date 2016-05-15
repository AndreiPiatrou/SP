using System;
using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;

using SP.Shell.Models;

namespace SP.Shell.ViewModel.Criteria
{
    public class DiscreteCriteriaRangeViewModel : ViewModelBase, ICriteriaRangeSelector
    {
        private readonly Action rangeChangedAction;
        private readonly Func<IEnumerable<string>> criteriaSelector;
        private readonly Func<RecordsCollection> collectionSelector;

        public DiscreteCriteriaRangeViewModel(
            Action rangeChangedAction,
            Func<IEnumerable<string>> criteriaSelector,
            Func<RecordsCollection> collectionSelector)
        {
            this.rangeChangedAction = rangeChangedAction;
            this.criteriaSelector = criteriaSelector;
            this.collectionSelector = collectionSelector;

            UpdateProperties();
        }

        public IEnumerable<SelectableValue> Values { get; private set; }

        public string SelectedValues
        {
            get
            {
                return Values == null
                           ? string.Empty
                           : string.Join(", ", Values.Where(v => v.Selected).Select(v => v.Value));
            }
        }

        public void Apply()
        {
            collectionSelector().Apply(Values.Where(v => v.Selected).Select(v => v.Value).ToList());
        }

        public bool CanApply()
        {
            return !string.IsNullOrWhiteSpace(SelectedValues);
        }

        private void UpdateProperties()
        {
            Values =
                criteriaSelector()
                    .Distinct()
                    .Where(v => !string.IsNullOrEmpty(v))
                    .Select(v => new SelectableValue(v, true, SelectionOnChange))
                    .ToList();

            RaisePropertyChanged(() => Values);
            RaisePropertyChanged(() => SelectedValues);
        }

        private void SelectionOnChange()
        {
            rangeChangedAction();
            RaisePropertyChanged(() => SelectedValues);
        }
    }
}