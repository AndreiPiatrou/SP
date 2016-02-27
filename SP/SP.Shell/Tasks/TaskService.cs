using System;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Extensions;

namespace SP.Shell.Tasks
{
    public class TaskService
    {
        public static void WrapToTask(Action execution)
        {
            WrapToTask(
                () =>
                    {
                        execution();

                        return true;
                    },
                b => { });
        }

        public static void WrapToTask<T>(Func<T> execution, Action<T> callback)
        {
            try
            {
                GetMessenger().SendLoader(true);
                Task.Factory.StartNew(execution).ContinueWith(
                    task =>
                        {
                            if (task.IsCompleted)
                            {
                                callback(task.Result);
                                GetMessenger().SendLoader(false);
                            }
                            else if (task.IsFaulted && task.Exception != null)
                            {
                                GetMessenger().SendLoader(false);
                                GetMessenger().SendException(task.Exception);
                            }

                            GetMessenger().SendLoader(false);
                        },
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                GetMessenger().SendException(ex);
            }
        }

        private static IMessenger GetMessenger()
        {
            return ServiceLocator.Current.GetInstance<Messenger>();
        }
    }
}
