using System;
using System.Collections.Generic;
using System.IO;

using SP.Shell.Services.FileReadService;

namespace SP.Shell.Services
{
    public class DataReadService
    {
        public IEnumerable<IEnumerable<string>> ReadFile(string path)
        {
            var extension = GetExtension(path);
            var service = GetService(extension);

            return service.Read(path);
        }

        private IFileReadService GetService(string extension)
        {
            switch (extension)
            {
                case ".csv":
                    return new CsvReadService();
                default:
                    throw new NotImplementedException();
            }
        }

        private string GetExtension(string path)
        {
            var extension = Path.GetExtension(path);
            if (extension == ".csv")
            {
                return extension;
            }

            throw new NotImplementedException();
        }
    }
}
