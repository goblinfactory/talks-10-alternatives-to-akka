using Akka.Actor;
using QuoteClient.Akka.Messages;
using QuoteShared;
using QuoteShared.UX;
using System.Collections.Generic;


namespace QuoteClient.Akka.Commands
{
    // BacklogManager
    // ------------
    // wait for the green light from the upstream monitor
    // manage the stream of incoming quotes (the backlog)
    // ensures all RFQ's get a quote Tracker to get quotes
    // as quotes come in, removes them from the backlog
    // track total quotes
    // total value of all quotes.

    public class BacklogManager : ReceiveActor
    {
        private IActorRef _userinterface;

        //private Dictionary<int, IActorRef> _quoteTrackers = new Dictionary<int, IActorRef>();

        private int _correlationId = 0;
        public BacklogManager()
        {
            Receive<RFQ>(message => {
                _correlationId++;
                _userinterface.Tell(new RfqReceived(message));
                _userinterface.Tell(new BacklogCountChanged(_correlationId));

            });

            Receive<UserInterfaceReady>(message =>
            {
               _userinterface = message.UX;
            });

        }

        public static IActorRef Create(ActorSystem system)
        {
            var actor = system.ActorOf(Props.Create(() => new BacklogManager()), NAME);
            return actor;
        }

        public static string NAME = "QuoteManager";
        
    }
}
