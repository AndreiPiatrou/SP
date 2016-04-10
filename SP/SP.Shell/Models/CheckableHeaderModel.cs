using System;

namespace SP.Shell.Models
{
    public class CheckableHeaderModel
    {
        private readonly Action onSelectionChange;

        private bool isChecked;

        public CheckableHeaderModel(string header, int index, Action onSelectionChange)
        {
            this.onSelectionChange = onSelectionChange;
            Header = header;
            Index = index;
        }

        public string Header { get; private set; }

        public int Index { get; private set; }

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
