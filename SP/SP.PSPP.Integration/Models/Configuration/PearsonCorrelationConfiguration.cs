using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class PearsonCorrelationConfiguration : ConfigurationBase
    {
        public PearsonCorrelationConfiguration(
            IEnumerable<VariableDescription> groupVariables,
            IEnumerable<VariableDescription> targetVariables)
            : base(groupVariables, targetVariables)
        {
        }
    }
}
