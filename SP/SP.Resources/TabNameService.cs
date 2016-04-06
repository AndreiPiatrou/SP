using System;
using System.Collections.Generic;
using System.Linq;

namespace SP.Resources
{
    public class TabNameService
    {
        private int resultsCounter;

        public string GetResults()
        {
            if (resultsCounter == 0)
            {
                ++resultsCounter;

                return Strings.Result;
            }

            return string.Format("{0} ({1})", Strings.Result, resultsCounter++);
        }

        public string GetImportedData(IEnumerable<string> existingNames)
        {
            var names = existingNames as IList<string> ?? existingNames.ToList();
            var counter = 0;
            var name = Strings.NewTab;

            while (names.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                name = string.Format("{0} ({1})", Strings.NewTab, ++counter);
            }

            return name;
        }
    }
}
