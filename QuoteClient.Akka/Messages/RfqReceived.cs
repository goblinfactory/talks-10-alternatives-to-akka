using QuoteShared;

namespace QuoteClient.Akka.Messages
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
