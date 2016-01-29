using System;

namespace SP.Shell.Messages
{
    public class OpenFileMessage
    {
        public OpenFileMessage(Action<string, string> positiveCallback)
        {
            PositiveCallback = positiveCallback;
        }

        public Action<string, string> PositiveCallback { get; private set; }
    }
}
