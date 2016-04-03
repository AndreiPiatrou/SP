using System.Collections.Generic;

using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Models
{
    public class InputData
    {
        public IEnumerable<IEnumerable<string>> Rows { get; set; } 

        public IConfiguration Configuration { get; set; }
    }
}
