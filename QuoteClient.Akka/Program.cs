using QuoteShared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuoteClient.Akka
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var upstream = QuoteStreamSimulator.GetDemoQuoteStream(totalRfqs: 300, pause: 100, spread: 1500);

            // wire up a Konsole window for queue of quotes
            // wire up a Konsole window for quote results

            await foreach (var quote in upstream)
            {
                // push all the quotes onto the backlog
            }
        }
    }



    // ACTORS   

    // QuoteFetcher
    // -------------
    // responsible for handling a quote for a provider 

    // QuoteRouter
    // -----------
    // responsible for scaling out requests for quotes to providers, so that the requests to a single provider can be asynchronous.

    // QuoteManager
    // ------------
    // takes a rfq and a 'panel' of providers
    // creates a 'child' QuoteFetcher actor for each provider, via a QuoteRouter
    // decides what to do when a child actor fails
    
    // QuoteTracker
    // ------------
    // tracks all quotes that come in for a specific RFQ 
    // as soon as we have 10 quotes in, tell the quote manager we dont need any more quotes.
    // if all quotes are in or have failed and we don't have 10
    // tell the quote manager when the job is done, or has failed.
    // tell the quotetracker our % progress

    // QuoteStreamManager
    // ------------------
    // manage the stream of incoming quotes
    // ensure all RFQ's get quotes
    // track the overall state of the quotes
    // update the UI to track the overall quotes progress
    // track active quotes
    // track total quotes
    // total value of all quotes.

    // QuoteUI
    // -------
    // simplest
    // 3 columns, backlog, busy, quotes (top and bottom)
    // quotes top - quote stream for all quotes
    // quotes bottom - stream of finished quotes, with best quote.


}
