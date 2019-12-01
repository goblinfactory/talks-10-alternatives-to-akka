using Bogus;
using Bogus.DataSets;
using QuoteShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteShared
{
    //NOT USED? CAN I DELETE
    public static class QuoteGenerator
    {
        private static Random _rnd = new Random();

        public static async IAsyncEnumerable<RFQ> GetDemoQuoteStream(int totalRfqs, int pause, int spread)
        {
            var rfqs = GenerateRFQs(totalRfqs);

            foreach (var rfq in rfqs)
            {
                yield return rfq;
                await Task.Delay(pause + _rnd.Next(spread));
            }
        }

        public static RFQ GenerateQuote()
        {
            var r = new Random();
            var f = new Faker("en");
            var p = f.Person;
            var rfq = new RFQ(
                driver: $"{p.FullName}",
                age: r.Next(40) + 18,
                regNo: Bogus.Extensions.UnitedKingdom
                    .RegistrationPlateExtensionsForGreatBritain
                    .GbRegistrationPlate(new Vehicle(), new DateTime(2002, 1, 1), new DateTime(2019, 1, 1))
            );
            return rfq;
        }

        //NOT USED? CAN I DELETE
        internal static IEnumerable<RFQ> GenerateRFQs(int cnt)
        {
            var quotes = Enumerable.Range(1, cnt).Select(i => GenerateQuote());
            return quotes;
        }

    }
}
