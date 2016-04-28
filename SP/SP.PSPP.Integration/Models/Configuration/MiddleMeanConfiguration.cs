using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class MiddleMeanConfiguration : ConfigurationBase
    {
        public MiddleMeanConfiguration(
            IEnumerable<VariableDescription> groupVariables,
            IEnumerable<VariableDescription> targetVariables)
            : base(groupVariables, targetVariables)
        {
        }
    }
}