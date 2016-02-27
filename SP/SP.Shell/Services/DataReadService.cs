using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SP.Shell.Services.FileReadService;

namespace SP.Shell.Services
{
    public class DataReadService
    {
        public IEnumerable<IEnumerable<IEnumerable<string>>> ReadWorksheets(string path)
        {
            var extension = GetExtension(path);
            var service = GetService(extension);
            var worksheetIndex = 0;

            while (service.WorksheetExists(path, worksheetIndex))
            {
                yield return service.Read(path, worksheetIndex).ToList();
                worksheetIndex++;
            }
        }

        private IFileReadService GetService(string extension)
        {
            switch (extension)
            {
                case ".csv":
                    return new CsvReadService();
                case ".xls":
                case ".xlsx":
                    return new ExcelDataReadService();
                default:
                    throw new NotImplementedException();
            }
        }

        private string GetExtension(string path)
        {
            var extension = Path.GetExtension(path);
            switch (extension)
            {
                case ".csv":
                    return extension;
                case ".xls":
                case ".xlsx":
                    return extension;
            }

            throw new NotImplementedException();
        }
    }
}
