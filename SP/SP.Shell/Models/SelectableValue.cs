namespace SP.Shell.Models
{
    public class SelectableValue
    {
        public SelectableValue(string value, bool selected)
        {
            Value = value;
            Selected = selected;
        }

        public string Value { get; private set; }

        public bool Selected { get; set; }
    }
}
