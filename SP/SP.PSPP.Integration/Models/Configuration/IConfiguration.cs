using System.Collections.Generic;

namespace SP.PSPP.Integration.Models.Configuration
{
    public interface IConfiguration
    {
        IEnumerable<string> Headers { get; } 
    }
}
