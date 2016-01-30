using System.Windows;
using System.Windows.Controls;

using GalaSoft.MvvmLight.Command;

using MahApps.Metro.Controls;

namespace SP.Shell.Controls
{
    public partial class FileActions
    {
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

        private void NotifyOnInternalButtonClick()
        {
            foreach (var button in this.FindChildren<Button>())
            {
                button.Click += (sender, args) => parent.IsOpen = false;
            }
        }
    }
}
