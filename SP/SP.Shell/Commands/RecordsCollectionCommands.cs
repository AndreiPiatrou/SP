using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using SP.Shell.Models;

namespace SP.Shell.Commands
{
    public class RecordsCollectionCommands
    {
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static ICommand RemoveRowCommand = new GuiListenCommand(RemoveRowExecute, RemoveRowCanExecute);

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static ICommand RemoveColumnCommand = new GuiListenCommand(RemoveColumnExecute, RemoveColumnCanExecute);

        private static void RemoveColumnExecute(object o)
        {
            var data = (RecordsCollection)o;

            data.RemoveColumn(data.SelectedHeader);
        }

        private static void RemoveRowExecute(object o)
        {
            var data = (RecordsCollection)o;

            data.RemoveRow(data.SelectedRow);
        }

        private static bool RemoveColumnCanExecute(object o)
        {
            var data = o as RecordsCollection;

            if (data == null)
            {
                Debug.WriteLine("Data is null");

                return false;
            }

            return data.SelectedHeader >= 0 && data.SelectedHeader <= data.Headers.Count - 2;
        }

        private static bool RemoveRowCanExecute(object o)
        {
            var data = o as RecordsCollection;

            if (data == null)
            {
                return false;
            }

            return data.SelectedRow >= 0 && data.SelectedRow <= data.Records.Count - 2;
        }
    }
}
