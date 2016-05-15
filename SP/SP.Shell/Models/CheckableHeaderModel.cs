using System;
using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight;

namespace SP.Shell.Models
{
    public class CheckableHeaderModel : ViewModelBase
    {
        private readonly Action onSelectionChange;

        private bool isChecked;

        private IEnumerable<string> values;

        private bool enabled;

        public CheckableHeaderModel(string header, int index, Action onSelectionChange)
        {
            this.onSelectionChange = onSelectionChange;
            Header = header;
            Index = index;
            Enabled = true;
        }

        public string Header { get; private set; }

        public int Index { get; private set; }

        public IEnumerable<string> Values
        {
            get
            {
                return values;
            }

            set
            {
                values = value.Distinct().Where(s => !string.IsNullOrEmpty(s));
                SelectedValue = values.FirstOrDefault();
            }
        }

        public string SelectedValue { get; set; }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                RaisePropertyChanged(() => Enabled);
            }
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
                onSelectionChange();
            }
        }
    }
}
