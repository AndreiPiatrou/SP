using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public interface IConfiguration
    {
        IEnumerable<VariableDescription> GroupVariables { get; }

        IEnumerable<VariableDescription> TargetVariables { get; }

        IEnumerable<VariableDescription> AllVariables { get; }

        bool HasGroups { get; }

        IEnumerable<string> GetAllHeaders();

        IEnumerable<GroupDescription> GetGroups();
    }
}
