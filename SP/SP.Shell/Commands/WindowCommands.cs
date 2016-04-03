using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using MahApps.Metro.SimpleChildWindow;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Messages;
using SP.Shell.Views;

namespace SP.Shell.Commands
{
    public class WindowCommands
    {
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static ICommand CloseWindowCommand = new RelayCommand<FrameworkElement>(CloseElement);

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static ICommand CloseApplication = new RelayCommand(() => Application.Current.Shutdown());

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
        public static ICommand AboutCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(SendOpenAboutMessage);

        private static void SendOpenAboutMessage()
        {
            ServiceLocator.Current.GetInstance<Messenger>().Send(new OpenChildWindowMessage(new About()));
        }

        private static void CloseElement(FrameworkElement element)
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
