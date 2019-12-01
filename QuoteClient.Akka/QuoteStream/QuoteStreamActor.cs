using Akka.Actor;
using QuoteClient.Akka.UserInterface;
using QuoteShared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuoteClient.Akka.QuoteStream
{
    public class SendRfq
    {
        public SendRfq()
        {

        }
    }
    
    public class QuoteStreamActor: ReceiveActor
    {
        private readonly int _totalRFQs;
        private readonly int _pauseBetweenSends;
        
        private readonly IActorRef _quoteManager;
        private readonly Func<RFQ> _generateRFQ;
        private readonly IActorRef _userInterface;
        private int _count = 0;
        private int _spreadStart;
        private int _spread;

        public QuoteStreamActor(IActorRef userInterface, int totalRFQs, int pauseBetweenSends, int spread, IActorRef quoteManager, Func<RFQ> generateTestData)
        {
            _totalRFQs = totalRFQs;
            _pauseBetweenSends = pauseBetweenSends;
            _spread = spread;
            _spreadStart = spread;
            _quoteManager = quoteManager;
            _generateRFQ = generateTestData;
            _userInterface = userInterface;
            
            Receive<Start>(message => {
                Become(Streaming);
                Self.Tell(new SendRfq());
            });

            Receive<Stop>(message => Become(Paused));
        }

        public void Streaming()
        {
            Receive<SendRfq>(message => {
                if ((_count++) >= _totalRFQs) {
                    Become(Paused);
                }
                Task.Run(() =>
                {
                    var rnd = new Random();
                    var delay = _pauseBetweenSends + rnd.Next(_spread);
                    _spread = _spread - 35;
                    _userInterface.Tell(new SpreadChanged(_spread));
                    if (_spread<50)
                    {
                        // do this to simulate large groups of users arriving, e.g. lunchtime
                        _spread = _spreadStart;
                    }
                    Task.Delay(delay).Wait();
                    _quoteManager.Tell(_generateRFQ());
                    return new SendRfq();
                }).PipeTo(Self);
            });
        }

        public void Paused()
        {
            // do nothing, we swallow any unprocessed message.
            Receive<SendRfq>(message => { });
        }

        public static IActorRef Create(ActorSystem system, IActorRef quoteManager, IActorRef userInterface, int totalRFQsToSend, int pauseBetweenSends, int spread,  Func<RFQ> generateTestData)
        {
            var actor = system.ActorOf(Props.Create(() => new QuoteStreamActor(userInterface, totalRFQsToSend, pauseBetweenSends, spread, quoteManager, generateTestData )), NAME);
            return actor;
        }

        public static string NAME = "QuoteStream";
    }
}

// references
// https://petabridge.com/blog/akka-actors-finite-state-machines-switchable-behavior/
// https://petabridge.com/blog/akkadotnet-async-actors-using-pipeto/