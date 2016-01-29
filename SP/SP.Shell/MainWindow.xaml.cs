using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

using MahApps.Metro.Controls.Dialogs;

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
        }
    }
}
