using System.Collections.Generic;
using System.IO;

using SP.PSPP.Integration.Models;

namespace SP.PSPP.Integration.Services
{
    public class InputDataWriter
    {
        public string WriteToFile(InputData data, string filePath)
        {
            var lines = GetDataLines(data);

            return WriteLinesToFile(lines, filePath);
        }

        private string WriteLinesToFile(IEnumerable<string> lines, string filePath)
        {
            File.WriteAllLines(filePath, lines);

            return filePath;
        }

        private IEnumerable<string> GetDataLines(InputData data)
        {
            yield return string.Join(",", data.Configuration.GetAllHeaders());

            foreach (var row in data.Rows)
            {
                yield return string.Join(",", row);
            }
        }
    }
}
