namespace QuoteClient.Akka.UserInterface
{
    public class BacklogCountChanged
    {
        public BacklogCountChanged(int newValue)
        {
            NewValue = newValue;
        }

        public int NewValue { get; }
    }
}
