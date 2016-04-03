using MahApps.Metro.SimpleChildWindow;

namespace SP.Shell.Messages
{
    public class OpenChildWindowMessage
    {
        public OpenChildWindowMessage(ChildWindow window)
        {
            Window = window;
        }

        public ChildWindow Window { get; private set; }
    }
}