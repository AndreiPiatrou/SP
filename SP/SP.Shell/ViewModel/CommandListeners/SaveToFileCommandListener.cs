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
            var writer = ServiceLocator.Current.GetInstance<DataWriteService>();

            TaskService.WrapToTask(() => writer.WriteToFile(message.Records.Rows, message.FilePath));
        }
    }
}
