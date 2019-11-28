using QuoteService.Internal;
using System;

namespace QuoteService.Models
{

    // todo swap out the configured serializer so that we can use immutable dto's
    // for now, allow them to be modified.

    public class Quote
    {
        public Quote()
        {
            Info = new CarInsuranceRequest();
        }
        public Quote(Guid id, decimal amount, string underwriter, CarInsuranceRequest info)
        {
            Id = id;
            Amount = amount;
            Info = info;
            Underwriter = underwriter;
        }

        public string Underwriter { get; set; }

        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public CarInsuranceRequest Info { get; set; }
        public override string ToString()
        {
            return $"{Info.Driver,-20} {Info.Car,-10} {Amount.ToCurrency()} {Underwriter,-15}";
        }
    }
}

