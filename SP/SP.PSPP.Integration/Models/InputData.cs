using System.Collections.Generic;

using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Models
{
    public class InputData
    {
        public InputData(IEnumerable<IEnumerable<string>> rows, IConfiguration configuration)
        {
            Rows = rows;
            Configuration = configuration;
        }

        public IEnumerable<IEnumerable<string>> Rows { get; private set; } 

        public IConfiguration Configuration { get; private set; }
    }
}
