using System.Windows.Controls;

using MahApps.Metro.Controls;

namespace SP.Shell.Controls
{
    public partial class AnalyzeActions
    {
        private Flyout parent;

        public AnalyzeActions()
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
