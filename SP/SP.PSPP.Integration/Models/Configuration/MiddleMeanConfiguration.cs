using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class MiddleMeanConfiguration : ConfigurationBase
    {
        public MiddleMeanConfiguration(
            IEnumerable<VariableDescription> groupVariables,
            VariableDescription targetVariable)
            : base(groupVariables, targetVariable)
        {
        }
    }
}