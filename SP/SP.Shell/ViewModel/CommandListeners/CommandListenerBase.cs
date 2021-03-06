using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Resources;
using SP.Shell.Services;

namespace SP.Shell.ViewModel.CommandListeners
{
    public abstract class CommandListenerBase : ViewModelBase
    {
        protected CommandListenerBase()
        {
            MessengerInstance = ServiceLocator.Current.GetInstance<Messenger>();

            // ReSharper disable once VirtualMemberCallInContructor
            SubscribeOnMessages();
        }

        protected MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }
        
        protected TabNameService TabNameService
        {
            get { return ServiceLocator.Current.GetInstance<TabNameService>(); }
        }

        protected AnalysisService AnalyzeService
        {
            get { return ServiceLocator.Current.GetInstance<AnalysisService>(); }
        }

        protected abstract void SubscribeOnMessages();
    }
}