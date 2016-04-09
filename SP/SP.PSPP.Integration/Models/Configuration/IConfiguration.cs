using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public interface IConfiguration
    {
        IEnumerable<VariableDescription> GroupVariables { get; }

        VariableDescription TargetVariable { get;}

        IEnumerable<VariableDescription> AllVariables { get; }

        IEnumerable<string> GetAllHeaders();

        IEnumerable<GroupDescription> GetGroups(IEnumerable<IEnumerable<string>> rows);
    }
}
