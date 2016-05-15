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

        public class CriteriaDescription
        {
            public CriteriaDescription(string name, IEnumerable<string> values, string targetValue)
            {
                Name = name;
                Values = values;
                TargetValue = targetValue;
            }

            public string Name { get; private set; }

            public IEnumerable<string> Values { get; private set; }

            public string TargetValue { get; private set; }
        }
    }
}
