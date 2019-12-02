using Akka.Actor;
using QuoteClient.Akka.Messages;
using QuoteShared;
using QuoteShared.UX;
using System.Collections.Generic;

namespace QuoteClient.Akka.Commands
{

    // QuoteTracker
    // ------------
    // tracks all quotes that come in for a specific RFQ 
    // as soon as we have 10 quotes in, tell the quote manager we dont need any more quotes.
    // if all quotes are in or have failed and we don't have 10
    // tell the quote manager when the job is done, or has failed.
    // tell the quotetracker our % progress
    public class QuoteTracker : ReceiveActor
    {
        private readonly IActorRef _userinterface;
        private Dictionary<int, IActorRef> _quoteTrackers = new Dictionary<int, IActorRef>();

        private int _correlationId = 0;
        public QuoteTracker(IActorRef userinterface)
        {
            _userinterface = userinterface;

            Receive<RFQ>(message => {
                _correlationId++;
                _userinterface.Tell(new RfqReceived(message));
                _userinterface.Tell(new BacklogCountChanged(_correlationId));

            });
        }

        public static IActorRef Create(ActorSystem system)
        {
            var actor = system.ActorOf(Props.Create(() => new BacklogManager()), NAME);
            return actor;
        }

        public static string NAME = "QuoteTracker";
        
    }
}
