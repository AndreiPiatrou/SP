using System.Collections.Generic;
using System.IO;

using CsvHelper;

namespace SP.Shell.Services.FileReadService
{
    public class CsvReadService : IFileReadService
    {
        public IEnumerable<IEnumerable<string>> Read(string path, int worksheetIndex)
        {
            var parser = CreateCsvParserFromFilePath(path);
            while (true)
            {
                var row = parser.Read();
                if (row == null)
                {
                    break;
                }

                yield return row;
            }
        }

        public bool WorksheetExists(string path, int worksheetIndex)
        {
            return worksheetIndex == 0;
        }

        private CsvParser CreateCsvParserFromFilePath(string path)
        {
            return new CsvParser(new StringReader(File.ReadAllText(path)));
        }
    }
}
