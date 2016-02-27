using System.Collections.Generic;

namespace SP.Shell.Services.FileReadService
{
    public interface IFileReadService
    {
        IEnumerable<IEnumerable<string>> Read(string path, int worksheetIndex);

        bool WorksheetExists(string path, int worksheetIndex);
    }
}