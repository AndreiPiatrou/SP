namespace SP.FIleSystem
{
    public class StatisticProcessRunResult
    {
        public StatisticProcessRunResult(bool success) :
            this(success, string.Empty)
        {
        }

        public StatisticProcessRunResult(string message) :
            this(false, message)
        {
        }

        public StatisticProcessRunResult(bool successed, string message)
        {
            Successed = successed;
            Message = message;
        }

        public string Message { get; private set; }

        public bool Successed { get; private set; }
    }
}
