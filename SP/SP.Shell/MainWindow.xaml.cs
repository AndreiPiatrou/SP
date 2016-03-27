using System.IO;
using System.Threading.Tasks;
using System.Windows;

using GalaSoft.MvvmLight.Messaging;

using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;

using SP.Resources;
using SP.Shell.Controls;
using SP.Shell.Messages;

namespace SP.Shell
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            RegisterForMessages(ServiceLocator.Current.GetInstance<Messenger>());
        }

        private void RegisterForMessages(IMessenger messenger)
        {
            messenger.Register<ShowPopupMessage>(
                this,
                async message => { await this.ShowMessageAsync(message.Title, message.Content); });
            messenger.Register<PrepareAnalyzeDataMessage>(
                this,
                async message => await OpenAnalyzeDataChildWindow(message));

            messenger.Register<OpenFileMessage>(this, OpenFile);
            messenger.Register<AskForFilePathMessage>(this, AskForFilePathMessage);
            messenger.Register<LoaderMessage>(this, ManageLoader);
            messenger.Register<OpenChildWindowMessage>(this, async message => { await OpenChildWindow(message); });
        }

        private void OpenFile(OpenFileMessage message)
        {
            var openFileDialog = new OpenFileDialog { Filter = Strings.FileFIlter };
            if (openFileDialog.ShowDialog() == true)
            {
                message.PositiveCallback(
                    openFileDialog.FileName,
                    Path.GetFileNameWithoutExtension(openFileDialog.FileName));
            }
        }

        private void AskForFilePathMessage(AskForFilePathMessage message)
        {
            var openFileDialog = new SaveFileDialog
            {
                DefaultExt = "csv",
                AddExtension = true,
                Filter = Strings.FileFIlter
            };

            if (openFileDialog.ShowDialog() == true)
            {
                message.PositiveCallback(openFileDialog.FileName);
            }
        }

        private async Task OpenAnalyzeDataChildWindow(PrepareAnalyzeDataMessage message)
        {
            var window = new AnalyzeDataWindow
            {
                DataContext = message.Model
            };

            await this.ShowChildWindowAsync(window, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void ManageLoader(LoaderMessage message)
        {
            LoaderGrid.Visibility = message.IsActive ? Visibility.Visible : Visibility.Collapsed;
            Loader.IsActive = message.IsActive;
        }

        private async Task OpenChildWindow(OpenChildWindowMessage message)
        {
            await this.ShowChildWindowAsync(message.Window, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }
    }
}
