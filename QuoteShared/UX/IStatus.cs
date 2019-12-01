namespace QuoteShared.UX
{
    public interface IStatus
    {
        int Backlog { get; }
        int Busy { get; }
        int Complete { get; }
        decimal TotalQuoteValue { get; }
        int Retries { get; }
        int Errors { get; }
    }


    public class Status : IStatus
    {
        public Status(int backlog, int busy, int complete, decimal totalQuoteValue, int retries, int errors)
        {
            Backlog = backlog;
            Busy = busy;
            Complete = complete;
            TotalQuoteValue = totalQuoteValue;
            Retries = retries;
            Errors = errors;
        }

        public int Backlog { get; }
        public int Busy { get; }
        public int Complete { get; }
        public decimal TotalQuoteValue { get; }
        public int Retries { get; }
        public int Errors { get; }

    }
}
