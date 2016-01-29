using System.Diagnostics.CodeAnalysis;
using System.Windows;

using GalaSoft.MvvmLight.Command;

using MahApps.Metro.SimpleChildWindow;

namespace SP.Shell.Commands
{
    public class WindowCommands
    {
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static RelayCommand<FrameworkElement> CloseWindowCommand = new RelayCommand<FrameworkElement>(Closeelement);

        private static void Closeelement(FrameworkElement element)
        {
            if (element is Window)
            {
                ((Window)element).Close();
            }
            else if (element is ChildWindow)
            {
                ((ChildWindow)element).Close();
            }
        }
    }
}
