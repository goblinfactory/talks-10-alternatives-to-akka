using System;

namespace QuoteShared
{

    // todo swap out the configured serializer so that we can use immutable dto's
    // for now, allow them to be modified.

    public class Quote
    {
        public Quote()
        {
            Info = new RFQ();
        }
        public Quote(Guid id, decimal amount, string underwriter, RFQ info)
        {
            Id = id;
            Amount = amount;
            Info = info;
            Underwriter = underwriter;
        }

        public string Underwriter { get; set; }

        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public RFQ Info { get; set; }
        public override string ToString()
        {
           return $"{Info.Driver,-20} {Info.RegNo,-10} {Amount.ToCurrency()} {Underwriter,-15}";
        }
    }
}

