using System;

using GalaSoft.MvvmLight.Messaging;

using SP.Resources;
using SP.Shell.Messages;

namespace SP.Shell.Extensions
{
    public static class MessengerExtensions
    {
        public static void SendException(this IMessenger messenger, Exception ex)
        {
            messenger.Send(new ShowPopupMessage(Strings.ErrorOccured, ex.ToString()));
        }

        public static void SendLoader(this IMessenger messenger, bool isActive)
        {
            messenger.Send(new LoaderMessage(isActive));
        }
    }
}
