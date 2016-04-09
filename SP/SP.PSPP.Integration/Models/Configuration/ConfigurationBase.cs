using System.Collections.Generic;
using System.Linq;

using SP.Extensions;

namespace SP.PSPP.Integration.Models.Configuration
{
    public abstract class ConfigurationBase : IConfiguration
    {
        private readonly EnumerableComparer comparer;

        protected ConfigurationBase(IEnumerable<VariableDescription> groupVariables, VariableDescription targetVariable)
        {
            GroupVariables = groupVariables;
            TargetVariable = targetVariable;

            comparer = new EnumerableComparer();
        }

        public virtual IEnumerable<VariableDescription> GroupVariables { get; private set; }

        public virtual VariableDescription TargetVariable { get; private set; }

        public virtual IEnumerable<VariableDescription> AllVariables
        {
            get { return GroupVariables.Concat(Enumerable.Repeat(TargetVariable, 1)).OrderBy(v => v.Index); }
        }

        public bool HasGroups
        {
            get { return GroupVariables.Any(); }
        }

        public virtual int TargetIndex
        {
            get { return AllVariables.ToList().IndexOf(TargetVariable); }
        }

        public virtual IEnumerable<string> GetAllHeaders()
        {
            return AllVariables.Select(v => v.Name);
        }

        public IEnumerable<GroupDescription> GetGroups(IEnumerable<IEnumerable<string>> rows)
        {
            var enumerable = rows as IList<IEnumerable<string>> ?? rows.ToList();

            var groups = enumerable.Select(r => r.Where((e, i) => i != TargetIndex)).Distinct(comparer).ToList();
            var criteriaValues = enumerable.Select(r => r.Where((e, i) => i == TargetIndex)).Select(r => r.First()).Distinct().ToList();

            return groups.Select(g => new GroupDescription(GroupVariables.ToList(), g.ToList(), criteriaValues, TargetVariable.Name));
        }
    }
}