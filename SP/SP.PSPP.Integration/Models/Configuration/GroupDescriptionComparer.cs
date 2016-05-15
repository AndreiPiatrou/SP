using System.Collections.Generic;
using System.Linq;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class GroupDescriptionComparer : IEqualityComparer<IEnumerable<VariableDescription>>
    {
        public bool Equals(IEnumerable<VariableDescription> left, IEnumerable<VariableDescription> right)
        {
            var leftArray = left as VariableDescription[] ?? left.ToArray();
            var rightArray = right as VariableDescription[] ?? right.ToArray();

            if (leftArray.Length != rightArray.Length)
            {
                return false;
            }

            return !leftArray.Where((t, i) => !Equals(leftArray.ElementAt(i), rightArray.ElementAt(i))).Any();
        }

        public int GetHashCode(IEnumerable<VariableDescription> obj)
        {
            return 0;
        }

        private bool Equals(VariableDescription x, VariableDescription y)
        {
            return x.TargetValue == y.TargetValue && x.Index == y.Index;
        }
    }
}