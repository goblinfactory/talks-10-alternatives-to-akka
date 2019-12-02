//using Akka.Actor;
//using QuoteClient.Akka.Commands;
//using System;
//using System.Linq;

//namespace QuoteClient.Akka.Actors
//{
//    public class KeyHandler : ReceiveActor
//    {
//        private readonly IActorRef _quoteStream;

//        public KeyHandler(IActorRef quoteStream)
//        {
//            _quoteStream = quoteStream;

         

//        }
//        public static IActorRef Create(ActorSystem system, IActorRef quoteStream)
//        {
//            var actor = system.ActorOf(Props.Create(() => new KeyHandler(quoteStream)), NAME);
//            return actor;
//        }

//        public static string NAME = "KeyHandledr";
//    }
//}
