using System.Collections.Generic;
using System.Linq;

namespace SP.PSPP.Integration.Models.Configuration
{
    public abstract class ConfigurationBase : IConfiguration
    {
        private readonly GroupDescriptionComparer comparer;

        protected ConfigurationBase(IEnumerable<VariableDescription> groupVariables, IEnumerable<VariableDescription> targetVariables)
        {
            GroupVariables = groupVariables;
            TargetVariables = targetVariables;

            comparer = new GroupDescriptionComparer();
        }

        public virtual IEnumerable<VariableDescription> GroupVariables { get; private set; }

        public IEnumerable<VariableDescription> TargetVariables { get; private set; }

        public virtual IEnumerable<VariableDescription> AllVariables
        {
            get { return GroupVariables.Concat(TargetVariables).OrderBy(v => v.Index); }
        }

        public bool HasGroups
        {
            get { return GroupVariables.Any(); }
        }

        public virtual IEnumerable<string> GetAllHeaders()
        {
            return AllVariables.Select(v => v.Name);
        }

        public IEnumerable<GroupDescription> GetGroups()
        {
            return ExtractGroupCombinations().Distinct(comparer).SelectMany(ConcatWithTarget);
        }

        private IEnumerable<GroupDescription> ConcatWithTarget(IEnumerable<VariableDescription> description)
        {
            return TargetVariables.Select(t => new GroupDescription(description.ToList(), t));
        }

        private IEnumerable<IEnumerable<VariableDescription>> ExtractGroupCombinations()
        {
            for (var i = 0; i < GroupVariables.First().Values.Count(); i++)
            {
                var i1 = i;
                yield return
                    GroupVariables.Select(
                        g =>
                        new VariableDescription(
                            g.Index,
                            g.Name,
                            g.IsNumeric,
                            Enumerable.Empty<string>(),
                            g.Values.ElementAt(i1)));
            }
        }
    }
}