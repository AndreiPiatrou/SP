using System.Collections.Generic;
using System.IO;

using Excel;

namespace SP.Shell.Services.FileReadService
{
    public class ExcelDataReadService : IFileReadService
    {
        public IEnumerable<IEnumerable<string>> Read(string path, int worksheetIndex)
        {
            var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            var excelReader = path.EndsWith("xls")
                                  ? ExcelReaderFactory.CreateBinaryReader(stream)
                                  : ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader = SwitchToWorksheet(excelReader, worksheetIndex);

            while (excelReader.Read())
            {
                var row = new List<string>();
                var column = 0;
                while (column < excelReader.FieldCount)
                {
                    row.Add(excelReader.GetString(column));
                    column++;
                }

                yield return row;
            }

            excelReader.Close();
        }

        public bool WorksheetExists(string path, int worksheetIndex)
        {
            var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            var excelReader = path.EndsWith("xls")
                                  ? ExcelReaderFactory.CreateBinaryReader(stream)
                                  : ExcelReaderFactory.CreateOpenXmlReader(stream);

            return excelReader.ResultsCount > worksheetIndex;
        }

        private IExcelDataReader SwitchToWorksheet(IExcelDataReader excelDataReader, int index)
        {
            var current = 0;
            while (current < index)
            {
                excelDataReader.NextResult();
                current++;
            }

            return excelDataReader;
        }
    }
}