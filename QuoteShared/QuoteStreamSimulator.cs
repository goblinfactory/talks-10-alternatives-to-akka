using Bogus;
using Bogus.DataSets;
using QuoteShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteShared
{
    public static class QuoteStreamSimulator
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
        internal static IEnumerable<RFQ> GenerateRFQs(int cnt)
        {
            var r = new Random();
            var quotes = Enumerable.Range(1, cnt).Select(i =>
            {
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
            });
            return quotes;
        }

    }
}
