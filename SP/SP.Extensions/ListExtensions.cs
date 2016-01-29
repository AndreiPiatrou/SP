using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SP.Extensions
{
    public static class ListExtensions
    {
        public static bool IsEmptyStringCollection(this IList<string> list)
        {
            return list.All(string.IsNullOrWhiteSpace);
        }

        public static ObservableCollection<string> ToObservable(this IEnumerable<string> list)
        {
            return new ObservableCollection<string>(list);
        }

        public static List<List<string>> NormalizeCollection(this List<List<string>> list)
        {
            var headersCount = list.Select(i => i.Count).Max();

            foreach (var row in list)
            {
                row.AddRange(Enumerable.Repeat(string.Empty, headersCount - row.Count));
            }

            return list;
        }
    }
}
