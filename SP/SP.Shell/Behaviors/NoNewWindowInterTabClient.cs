using System;
using System.Windows;

using Dragablz;

namespace SP.Shell.Behaviors
{
    public class NoNewWindowInterTabClient : IInterTabClient
    {
        public virtual INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var sourceWindow = Window.GetWindow(source);
            if (sourceWindow == null)
            {
                throw new ApplicationException("Unable to ascrtain source window.");
            }
            
            return new NewTabHost<Window>(sourceWindow, source);
        }

        public virtual TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            return TabEmptiedResponse.DoNothing;
        }
    }
}
