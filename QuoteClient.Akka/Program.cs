using Konsole;
using Konsole.Layouts;
using QuoteShared;
using QuoteShared.UX;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuoteClient.Akka
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var upstream = QuoteStreamSimulator.GetDemoQuoteStream(totalRfqs: 300, pause: 100, spread: 1000);

            var ux = new UserInterface();
            int i = 1;
            await foreach (var quote in upstream)
            {
                ux.Status.Refresh(new Status(i++, 0, 0, 0M, 0, 0));
                
                // print all the quotes as they come in onto the backlog so that they scroll into view
                ux.Backlog.WriteLine(quote.ToString());
            }
        }
    }



    // ACTORS   

    // PollingServiceChecker
    // ---------------------
    // checks the status of a service (polls), reports back to caller when services comes online, or goes down.
    // for now, we're only going to check for when services come online

    // PanelStatusActor
    // -------------
    // checks the status of ALL the services and notifies the quote manager when all services are ready.
    // if a services goes down, then tells the quoteManager to change status to OFFLINE

    // QuoteFetcher
    // -------------
    // responsible for handling a quote for a provider 

    // QuoteRouter
    // -----------
    // responsible for scaling out requests for quotes to providers, so that the requests to a single provider can be asynchronous.

    // QuoteManager
    // ------------
    // runs in two modes, OFFLINE and ONLINE 
    // when OFFLINE, all new RFQ's must be queued, when ONLINE, should process all queued messages. Lets see if there is default berhavior for this?
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
