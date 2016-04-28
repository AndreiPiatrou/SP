using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class MeanChanceConfiguration : ConfigurationBase
    {
        public MeanChanceConfiguration(
            IEnumerable<VariableDescription> groupVariables,
            IEnumerable<VariableDescription> targetVariables)
            : base(groupVariables, targetVariables)
        {
        }
    }
}