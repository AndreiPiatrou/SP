using System.Collections.Generic;

namespace SP.PSPP.Integration.Models
{
    public class OutputData
    {
        public IEnumerable<IEnumerable<string>> Rows { get; set; }
    }
}
