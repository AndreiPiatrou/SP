using System;

namespace SP.Shell.Messages
{
    public class AskForFilePathMessage
    {
        public AskForFilePathMessage(Action<string> positiveCallback)
        {
            PositiveCallback = positiveCallback;
        }

        public Action<string> PositiveCallback { get; private set; } 
    }
}
