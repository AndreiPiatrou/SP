using System.Collections.Generic;
using System.IO;

using CsvHelper;

namespace SP.Shell.Services.FileReadService
{
    public class CsvReadService : IFileReadService
    {
        public IEnumerable<IEnumerable<string>> Read(string path)
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

        private CsvParser CreateCsvParserFromFilePath(string path)
        {
            return new CsvParser(new StringReader(File.ReadAllText(path)));
        }
    }
}
