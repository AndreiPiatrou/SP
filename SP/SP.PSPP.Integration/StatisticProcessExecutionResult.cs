namespace SP.PSPP.Integration
{
    public class StatisticProcessExecutionResult
    {
        public StatisticProcessExecutionResult(bool success) :
            this(success, string.Empty)
        {
        }

        public StatisticProcessExecutionResult(string message) :
            this(false, message)
        {
        }

        public StatisticProcessExecutionResult(bool successed, string message)
        {
            Successed = successed;
            Message = message;
        }

        public string Message { get; private set; }

        public bool Successed { get; private set; }
    }
}
