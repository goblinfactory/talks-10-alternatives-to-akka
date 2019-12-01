using Akka.Actor;
using QuoteClient.Akka.UserInterface;
using QuoteShared;
using System;
using System.Collections.Generic;
using System.Text;

// BacklogManager
// ------------
// looks after the RFQ backlog.

// runs in two modes, OFFLINE and ONLINE 
// when OFFLINE, all new RFQ's must be queued, when ONLINE, should process all queued messages. Lets see if there is default berhavior for this?
// takes a rfq and a 'panel' of providers
// creates a 'child' QuoteFetcher actor for each provider, via a QuoteRouter
// decides what to do when a child actor fails
// manage the stream of incoming quotes
// ensure all RFQ's get quotes
// track the overall state of the quotes
// update the UI to track the overall quotes progress
// track active quotes
// track total quotes
// total value of all quotes.

namespace QuoteClient.Akka.BacklogManager
{
    public class BacklogManagerActor : ReceiveActor
    {
        private readonly IActorRef _userinterface;
        private int _backLogCount = 0;
        public BacklogManagerActor(IActorRef userinterface)
        {
            _userinterface = userinterface;

            Receive<RFQ>(message => {
                _userinterface.Tell(new RfqReceived(message));
                _userinterface.Tell(new BacklogCountChanged(++_backLogCount));
            });
            
        }

        public static IActorRef Create(ActorSystem system, IActorRef userinterface)
        {
            var actor = system.ActorOf(Props.Create(() => new BacklogManagerActor(userinterface)), NAME);
            return actor;
        }

        public static string NAME = "QuoteManager";
        
    }
}
