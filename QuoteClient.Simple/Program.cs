using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using QuoteShared;

namespace QuoteClient.Simple
{
    class Program
    {
        private static ConcurrentQueue<RFQ> _rfqs;
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var rfqs = QuoteGenerator.GetDemoQuoteStream(totalRfqs: 300, pause:100, spread:1500);

            // start background add RFQ's to queue as they come in
            var stream = Task.Run(async () =>
            {
                await foreach (var rfq in rfqs)
                {
                    _rfqs.Enqueue(rfq);
                }
            });

            //_rfqs = new ConcurrentQueue<RFQ>(rfqs);

            // if we get here we are done.

            // print out the grand total.

            // wait for the stream to finish sending us all the quotes

            stream.Wait();

            // need another means of tracking when we're done, possibly  a slimLock
            // so that all threads can finish processing all the quotes.

            while (!_rfqs.IsEmpty)
            {
                // wait for all threads to process all the quotes
            }


        }
    }
}
