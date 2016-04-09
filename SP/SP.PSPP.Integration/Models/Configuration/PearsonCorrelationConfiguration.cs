using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class PearsonCorrelationConfiguration : ConfigurationBase
    {
        public PearsonCorrelationConfiguration(
            IEnumerable<VariableDescription> groupVariables,
            VariableDescription targetVariable)
            : base(groupVariables, targetVariable)
        {
        }
    }
}
