using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public class MeanChanceConfiguration : ConfigurationBase
    {
        public MeanChanceConfiguration(
            IEnumerable<VariableDescription> groupVariables,
            VariableDescription targetVariable)
            : base(groupVariables, targetVariable)
        {
        }
    }
}