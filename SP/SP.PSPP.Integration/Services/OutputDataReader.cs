using System.IO;
using System.Linq;

using SP.PSPP.Integration.Models;

namespace SP.PSPP.Integration.Services
{
    public class OutputDataReader
    {
        public OutputData Read(string filePath)
        {
            return new OutputData
            {
                Rows = File.ReadAllLines(filePath).Select(line => line.Split(",".ToCharArray()))
            };
        }
    }
}
