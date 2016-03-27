using System.Windows;

using SP.Shell.Models;

namespace SP.Shell.Controls
{
    public partial class ContextActions
    {
        public static readonly DependencyProperty RecordsProperty = DependencyProperty.Register(
            "Records",
            typeof(RecordsCollection),
            typeof(ContextActions),
            new PropertyMetadata(default(RecordsCollection)));

        public ContextActions()
        {
            InitializeComponent();
        }

        public RecordsCollection Records
        {
            get { return (RecordsCollection)GetValue(RecordsProperty); }
            set { SetValue(RecordsProperty, value); }
        }
    }
}
