using QuoteShared;

namespace QuoteClient.Akka
{
    public class RfqReceived
    {
        public RfqReceived(RFQ rfq)
        {
            RFQ = rfq;
        }

        public RFQ RFQ { get; }
    }
}
