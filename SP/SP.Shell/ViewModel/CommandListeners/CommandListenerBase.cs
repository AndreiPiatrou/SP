using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

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

        protected abstract void SubscribeOnMessages();
    }
}