namespace SP.Shell.Messages
{
    public class LoaderMessage
    {
        public LoaderMessage(bool isActive)
        {
            IsActive = isActive;
        }

        public bool IsActive { get; private set; }
    }
}
