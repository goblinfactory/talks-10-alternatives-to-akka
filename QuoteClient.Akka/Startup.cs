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

            // trick to allow bidirectional dependancies,tell the other actors about the actor via a message
            // instead of via props.
            var uxReady = new UserInterfaceReady(ux);
            backLogManager.Tell(uxReady);
            quoteStream.Tell(uxReady);
            
            IActorRef program = ProgramActor.Create(system, ux, monitor);
            return program;
        }
    }
}
