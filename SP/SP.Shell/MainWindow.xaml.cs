﻿using System.IO;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;

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

            RegisterForMessages(SimpleIoc.Default.GetInstance<Messenger>());
        }

        private void RegisterForMessages(Messenger messenger)
        {
            messenger.Register<ShowPopupMessage>(
                this,
                async message => { await this.ShowMessageAsync(message.Title, message.Content); });
            messenger.Register<OpenFileMessage>(
                this,
                message =>
                    {
                        var openFileDialog = new OpenFileDialog();
                        if (openFileDialog.ShowDialog() == true)
                        {
                            message.PositiveCallback(
                                openFileDialog.FileName,
                                Path.GetFileNameWithoutExtension(openFileDialog.FileName));
                        }
                    });
            messenger.Register<AnalyzeDataMessage>(
                this,
                async message => await this.ShowChildWindowAsync(
                    new AnalyzeDataWindow
                    {
                        DataContext = message.Model
                    },
                    ChildWindowManager.OverlayFillBehavior.FullWindow));
        }
    }
}
