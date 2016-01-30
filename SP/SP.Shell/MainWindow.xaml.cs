using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using GalaSoft.MvvmLight.Messaging;

using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;

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
        }

        private void OpenFile(OpenFileMessage message)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                message.PositiveCallback(
                    openFileDialog.FileName,
                    Path.GetFileNameWithoutExtension(openFileDialog.FileName));
            }
        }

        private void AskForFilePathMessage(AskForFilePathMessage message)
        {
            var openFileDialog = new SaveFileDialog();
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
    }
}
