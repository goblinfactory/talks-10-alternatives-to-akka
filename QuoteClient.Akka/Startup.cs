using Akka.Actor;
using QuoteClient.Akka.Commands;
using QuoteShared;
using QuoteShared.UX;
using System.Threading;

namespace QuoteClient.Akka
{
    public static class Startup
    {
        /// <summary>
        /// Configure all the actors and dependancies, returns an IActorRef for the ProgramActor actor
        /// </summary>
        public static IActorRef ConfigureServices(ActorSystem system, InsuranceProvider[] providers,  Barrier barrier)
        {
            
            IActorRef backLogManager = BacklogManager.Create(system);
            IActorRef quoteStream = QuoteStream.Create(system, backLogManager, 100, 100, 1500, QuoteGenerator.GenerateQuote);
            var userInterface = new UserInterface();
            IActorRef ux = UIManager.Create(system, quoteStream, userInterface, barrier);
            IActorRef monitor = UpstreamMonitor.Create(system, quoteStream, userInterface.Panel, providers);

            // trick to allow bidirectional dependancies
            var uxReady = new UserInterfaceReady(ux);
            backLogManager.Tell(uxReady);
            quoteStream.Tell(uxReady);
            
            IActorRef program = ProgramActor.Create(system, ux, monitor);
            return program;
        }
    }


    // BacklogManager
    //--------------

    // UpstreamMonitor
    // -------------
    // checks the status of ALL the upstream services and notifies the quote manager when enough services are ready.
    // if a services goes down, then tells the quoteManager to change status to OFFLINE

    // QuoteFetcher
    // -------------
    // responsible for getting a single quote from a provider 

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
