using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class VariableDescription
    {
        public VariableDescription(int index, string name, bool isNumeric, IEnumerable<string> values)
            : this(index, name, isNumeric, values, string.Empty)
        {
        }

        public VariableDescription(
            int index,
            string name,
            bool isNumeric,
            IEnumerable<string> values,
            string targetValue)
        {
            Index = index;
            Name = name;
            IsNumeric = isNumeric;
            Values = values;
            TargetValue = targetValue;
        }

        public int Index { get; private set; }

        public string Name { get; private set; }

        public bool IsNumeric { get; private set; }

        public IEnumerable<string> Values { get; private set; }

        public string TargetValue { get; private set; }
    }
}