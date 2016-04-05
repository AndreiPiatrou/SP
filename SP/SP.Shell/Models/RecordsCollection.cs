using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using SP.Extensions;

namespace SP.Shell.Models
{
    public class RecordsCollection
    {
        private int columnsCounter = 1;

        public RecordsCollection()
        {
            Headers = new ObservableCollection<string>(GenerateHeader(0));
            Records = new ObservableCollection<ObservableCollection<string>>
                          {
                              new ObservableCollection<string>
                                  {
                                      string.Empty
                                  }
                          };
        }

        public RecordsCollection(List<List<string>> records)
        {
            records.Add(new List<string>());
            records = records.NormalizeCollection();
            ObservableCollection<string> potentialHeaders;
            var headerExtracted = TryExtractHeaders(records[0], out potentialHeaders);
            Headers = headerExtracted
                          ? new ObservableCollection<string>(potentialHeaders)
                          : new ObservableCollection<string>(GenerateHeader(records[0].Count));

            if (headerExtracted)
            {
                foreach (var record in records)
                {
                    record.Add(string.Empty);
                }
            }

            Records =
                new ObservableCollection<ObservableCollection<string>>(
                    records.Skip(headerExtracted ? 1 : 0).Select(i => new ObservableCollection<string>(i)));
        }

        public ObservableCollection<ObservableCollection<string>> Records { get; private set; }

        public ObservableCollection<string> Headers { get; private set; }

        public IEnumerable<IEnumerable<string>> Rows
        {
            get
            {
                yield return Headers.SkipLast();
                foreach (var record in Records.SkipLast())
                {
                    yield return record.SkipLast();
                }
            }
        }
        
        public int SelectedRow { get; set; }

        public int SelectedHeader { get; set; }

        public void UpdateRowsAndHeaders()
        {
            var lastColumn = Records.Select(r => r.Last()).ToList();
            if (!lastColumn.IsEmptyStringCollection())
            {
                Headers.Add(GenerateNextColumnName());
                foreach (var record in Records)
                {
                    record.Add(string.Empty);
                }

                UpdateRowsAndHeaders();
            }

            if (!Records.Last().IsEmptyStringCollection())
            {
                Records.Add(Enumerable.Repeat(string.Empty, Headers.Count).ToObservable());
                UpdateRowsAndHeaders();
            }
        }

        public void RemoveRow(int index)
        {
            if (index > Records.Count - 2 || index < 0)
            {
                return;
            }

            Records.RemoveAt(index);
        }

        public void RemoveColumn(int index)
        {
            if (index > Headers.Count - 2 || index < 0)
            {
                return;
            }

            Headers.RemoveAt(index);
        }

        public void RenameHeader(int index, string value)
        {
            Headers[index] = value;
        }

        private bool TryExtractHeaders(IList<string> firstLine, out ObservableCollection<string> headers)
        {
            headers = new ObservableCollection<string>(firstLine);
            headers.Add(GenerateNextColumnName());

            foreach (var columnName in firstLine)
            {
                double value;
                if (double.TryParse(columnName, out value))
                {
                    headers = null;
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<string> GenerateHeader(int currentCount)
        {
            for (var i = 0; i <= currentCount; i++)
            {
                yield return GenerateNextColumnName();
            }
        }

        private string GenerateNextColumnName()
        {
            var name = "Variable" + columnsCounter;
            ++columnsCounter;

            return name;
        }
    }
}
