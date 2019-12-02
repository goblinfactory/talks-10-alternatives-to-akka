using Akka.Actor;
using QuoteShared;
using QuoteShared.UX;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuoteClient.Akka.Commands
{

    public class QuoteStream: ReceiveActor
    {
        private readonly int _totalRFQs;
        private readonly int _pauseBetweenSends;
        
        private readonly IActorRef _quoteManager;
        private readonly Func<RFQ> _generateRFQ;
        private IActorRef _userInterface;
        private int _count = 0;
        private int _spreadStart;
        private int _spread;

        public QuoteStream(IActorRef quoteManager, int totalRFQs, int pauseBetweenSends, int spread, Func<RFQ> generateTestData)
        {
            _totalRFQs = totalRFQs;
            _pauseBetweenSends = pauseBetweenSends;
            _spread = spread;
            _spreadStart = spread;
            _quoteManager = quoteManager;
            _generateRFQ = generateTestData;

            Receive<UserInterfaceReady>(message =>
            {
                _userInterface = message.UX;
            });

            Receive<UpstreamONLINE>(message => {
                Become(Streaming);
                Self.Tell(new SendRfq());
            });

            Receive<UpstreamOFFLINE>(message => Become(Paused));
        }

        public void Streaming()
        {
            Receive<ToggleQuoteStream>(message => {
                Become(Paused);
                _userInterface.Tell("paused quote stream.");
            });

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
            Receive<ToggleQuoteStream>(message => {
                Become(Streaming);
                Self.Tell(new SendRfq());
                _userInterface?.Tell("enabled streaming.");
            });
            // do nothing, we swallow any unprocessed message.
            Receive<SendRfq>(message => { });
        }

        public static IActorRef Create(ActorSystem system, IActorRef backlogManager, int totalRFQsToSend, int pauseBetweenSends, int spread,  Func<RFQ> generateTestData)
        {
            var actor = system.ActorOf(Props.Create(() => new QuoteStream(backlogManager, totalRFQsToSend, pauseBetweenSends, spread, generateTestData )), NAME);
            return actor;
        }

        public static string NAME = "QuoteStream";
    }
}