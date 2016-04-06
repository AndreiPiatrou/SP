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
            return GetImportedData(existingNames, Strings.NewTab);
        }

        public string GetImportedData(IEnumerable<string> existingNames, string newName)
        {
            var names = existingNames as IList<string> ?? existingNames.ToList();
            var counter = 0;
            var name = newName;

            while (names.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                name = string.Format("{0} ({1})", newName, ++counter);
            }

            return name;
        }
    }
}
