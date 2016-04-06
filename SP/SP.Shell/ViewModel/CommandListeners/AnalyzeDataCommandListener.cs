using System;

using SP.Extensions;
using SP.Resources;
using SP.Shell.Messages;

namespace SP.Shell.ViewModel.CommandListeners
{
    public class AnalyzeDataCommandListener : CommandListenerBase
    {
        protected override void SubscribeOnMessages()
        {
            MessengerInstance.Register<AnalyzeDataMessage>(this, AnalyzeDataExecute);
        }

        private void AnalyzeDataExecute(AnalyzeDataMessage message)
        {
            try
            {
                var result = AnalyzeService.Analyze(message.InputData, message.Type);
                var newTabName = TabNameService.GetResults();
                var resultList = result.Rows.ToCompleteList();
                var newTab = new TabViewModel(newTabName, resultList);

                Main.AddAndSelectTab(newTab);
            }
            catch (Exception e)
            {
                MessengerInstance.Send(new ShowPopupMessage(Strings.ErrorOccured, e.Message));
            }
        }
    }
}
