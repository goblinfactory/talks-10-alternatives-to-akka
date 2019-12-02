namespace QuoteShared.UX
{
    public class SpreadChanged
    {
        public int NewValue { get; }
        public SpreadChanged(int value)
        {
            NewValue = value;
        }
    }
}
