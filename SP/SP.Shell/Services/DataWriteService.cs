using System.Collections.Generic;
using System.IO;
using System.Linq;

using SP.Extensions;

namespace SP.Shell.Services
{
    public class DataWriteService
    {
        public void WriteToFile(IEnumerable<IEnumerable<string>> dataRows, string filePath)
        {
            dataRows = dataRows.ToCompleteList().NormalizeCollection();
            File.WriteAllLines(filePath, dataRows.Select(row => string.Join(",", row.Select(r => "\"" + r + "\""))));
        }
    }
}
