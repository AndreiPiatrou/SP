using System.Collections.Generic;
using System.Linq;

namespace SP.Extensions
{
    public class EnumerableComparer : IEqualityComparer<IEnumerable<string>>
    {
        public bool Equals(IEnumerable<string> x, IEnumerable<string> y)
        {
            var enumerableLeft = x as IList<string> ?? x.ToList();
            var enumerableRight = y as IList<string> ?? y.ToList();

            if (enumerableLeft.Count != enumerableRight.Count)
            {
                return false;
            }

            return !enumerableLeft.Where((t, i) => t != enumerableRight[i]).Any();
        }

        public int GetHashCode(IEnumerable<string> obj)
        {
            return 0;
        }
    }
}
