using System.Collections.Generic;
using System.Linq;

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

        protected override IEnumerable<GroupDescription> ConcatWithTarget(IEnumerable<VariableDescription> description)
        {
            return
                Enumerable.Repeat(
                    new PearsonCorrelationGroupDescription(description.ToList(), TargetVariables.ToList()),
                    1);
        }
    }
}
