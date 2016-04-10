using Microsoft.Practices.ServiceLocation;

using SP.Shell.Messages;
using SP.Shell.Services;
using SP.Shell.Tasks;

namespace SP.Shell.ViewModel.CommandListeners
{
    public class SaveToFileCommandListener : CommandListenerBase
    {
        protected override void SubscribeOnMessages()
        {
            MessengerInstance.Register<SaveRecordsToFileMessage>(this, HandleSaveToFileMessage);
        }

        private void HandleSaveToFileMessage(SaveRecordsToFileMessage message)
        {
            TaskService.WrapToTask(
                () =>
                ServiceLocator.Current.GetInstance<DataWriteService>()
                    .WriteToFile(message.Records.Rows, message.FilePath));
        }
    }
}
