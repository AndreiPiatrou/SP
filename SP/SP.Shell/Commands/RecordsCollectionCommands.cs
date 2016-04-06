using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Messages;
using SP.Shell.Models;
using SP.Shell.ViewModel;
using SP.Shell.Views;

namespace SP.Shell.Commands
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]

    public class RecordsCollectionCommands
    {
        public static ICommand RemoveRowCommand = new GuiListenCommand(RemoveRowExecute, RemoveRowCanExecute);

        public static ICommand RemoveColumnCommand = new GuiListenCommand(RemoveColumnExecute, RemoveColumnCanExecute);

        public static ICommand RenameColumnCommand = new GuiListenCommand(RenameColumnExecute, RenameColumnCanExecute);

        public static ICommand InsertColumnCommand = new GuiListenCommand(InsertColumnExecute, InsertColumnCanExecute);

        public static ICommand InsertRowCommand = new GuiListenCommand(InsertRowExecute, InsertRowCanExecute);

        private static bool InsertRowCanExecute(object o)
        {
            var data = (RecordsCollection)o;

            return data != null && data.SelectedRow > -1;
        }

        private static void InsertRowExecute(object o)
        {
            var data = (RecordsCollection)o;

            data.InsertRow(data.SelectedRow);
        }

        private static bool InsertColumnCanExecute(object o)
        {
            var data = (RecordsCollection)o;

            return data != null && data.SelectedHeader > -1;
        }

        private static void InsertColumnExecute(object o)
        {
            var data = (RecordsCollection)o;

            data.InsertHeader(data.SelectedHeader);
        }

        private static bool RenameColumnCanExecute(object o)
        {
            var data = (RecordsCollection)o;

            return data != null && data.SelectedHeader > -1;
        }

        private static void RenameColumnExecute(object o)
        {
            var data = (RecordsCollection)o;
            var window = new EditStringWindow
            {
                DataContext =
                                     new EditStringViewModel(
                                     s => data.RenameHeader(data.SelectedHeader, s),
                                     data.Headers[data.SelectedHeader])
            };

            GetMessenger().Send(new OpenChildWindowMessage(window));
        }

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

        private static IMessenger GetMessenger()
        {
            return ServiceLocator.Current.GetInstance<Messenger>();
        }
    }
}
