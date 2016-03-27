using System.Windows.Controls;

using MahApps.Metro.Controls;

namespace SP.Shell.Controls
{
    public partial class AboutActions
    {
        private Flyout parent;

        public AboutActions()
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
