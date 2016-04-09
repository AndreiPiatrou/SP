using SP.PSPP.Integration.Models.Configuration;

namespace SP.PSPP.Integration.Extensions
{
    public static class VariableDescriptionExtensions
    {
        public static string GetVariableDefinition(this VariableDescription variable)
        {
            return variable.Name + (variable.IsNumeric ? " F10.10" : " A20");
        }
    }
}
