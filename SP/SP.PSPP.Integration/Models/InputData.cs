using System.Collections.Generic;

namespace SP.PSPP.Integration.Models
{
    public class InputData
    {
        public IEnumerable<string> Variables { get; set; }

        public IEnumerable<IEnumerable<string>> Rows { get; set; } 
    }
}
