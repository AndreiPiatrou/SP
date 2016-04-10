namespace SP.PSPP.Integration.Models.Configuration
{
    public class VariableDescription
    {
        public VariableDescription(int index, string name, bool isNumeric)
        {
            Index = index;
            Name = name;
            IsNumeric = isNumeric;
        }

        public int Index { get; set; }

        public string Name { get; set; }

        public bool IsNumeric { get; set; }
    }
}