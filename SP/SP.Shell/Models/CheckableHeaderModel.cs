using System;
using System.Collections.Generic;
using System.Linq;

namespace SP.Shell.Models
{
    public class CheckableHeaderModel
    {
        private readonly Action onSelectionChange;

        private bool isChecked;

        private IEnumerable<string> values;

        public CheckableHeaderModel(string header, int index, Action onSelectionChange)
        {
            this.onSelectionChange = onSelectionChange;
            Header = header;
            Index = index;
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
