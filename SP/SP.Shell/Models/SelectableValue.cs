using System;

namespace SP.Shell.Models
{
    public class SelectableValue
    {
        private readonly Action onChange;

        private bool selected;

        public SelectableValue(string value, bool selected, Action onChange)
        {
            this.onChange = onChange;
            this.selected = selected;
            Value = value;
        }

        public string Value { get; private set; }

        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                selected = value;
                onChange();
            }
        }
    }
}
