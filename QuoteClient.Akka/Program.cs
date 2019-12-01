using Akka.Actor;
using QuoteClient.Akka.BacklogManager;
using QuoteClient.Akka.QuoteStream;
using QuoteClient.Akka.UserInterface;
using QuoteClient.Akka.UX;
using QuoteShared;
using QuoteShared.UX;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuoteClient.Akka
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var system = ActorSystem.Create("GoblinfactoryInsuranceQuotes"))
            {
                Thread.Sleep(500);

                IActorRef ux = UserInterfaceActor.Create(system);
                IActorRef backLogManager = BacklogManagerActor.Create(system, ux);
                IActorRef quoteStream = QuoteStreamActor.Create(system, backLogManager, ux, 100, 100, 1500,  QuoteGenerator.GenerateQuote);
                
                quoteStream.Tell(new Start());

                ux.Tell("press any key to quit.");
                Console.ReadKey(false);
                await system.Terminate();
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

    // QuoteTracker
    // ------------
    // tracks all quotes that come in for a specific RFQ 
    // as soon as we have 10 quotes in, tell the quote manager we dont need any more quotes.
    // if all quotes are in or have failed and we don't have 10
    // tell the quote manager when the job is done, or has failed.
    // tell the quotetracker our % progress
       
    // UserInterfaceActor
    // ------------------
    // does UI stuff
    
    // TIME PERMITTING configure Akka to deploy the UI remotely.
    
}
