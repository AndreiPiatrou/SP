using System.Windows;
using System.Windows.Controls;

using GalaSoft.MvvmLight.Command;

using MahApps.Metro.Controls;

namespace SP.Shell.Controls
{
    public partial class FileActions
    {
        public static readonly DependencyProperty OpenFileCommandProperty =
            DependencyProperty.Register(
                "OpenFileCommand",
                typeof(RelayCommand),
                typeof(FileActions),
                new PropertyMetadata(default(RelayCommand)));

        private Flyout parent;

        public FileActions()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
                {
                    NotifyOnInternalButtonClick();
                    parent = this.GetParentObject() as Flyout;
                };
        }

        public RelayCommand OpenFileCommand
        {
            get { return (RelayCommand)GetValue(OpenFileCommandProperty); }
            set { SetValue(OpenFileCommandProperty, value); }
        }

        private void NotifyOnInternalButtonClick()
        {
            foreach (var button in this.FindChildren<Button>())
            {
                button.Click += (sender, args) => parent.IsOpen = false;
            }
        }
    }
}
