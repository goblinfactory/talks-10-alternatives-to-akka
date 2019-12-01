using Akka.Actor;
using System;

namespace QuoteClient.Akka.UserInterface
{
    public class UserInterfaceActor : ReceiveActor
    {
        private readonly QuoteShared.UserInterface _ux;
        public UserInterfaceActor(QuoteShared.UserInterface ux)
        {
            _ux = ux;

            Receive<BacklogCountChanged>(message =>
            {
                _ux.Status.Update(message);
            });

            Receive<RfqReceived>(message =>
            {
                _ux.Backlog.Write(ConsoleColor.Cyan, $"{DateTime.Now.ToString("hh:mm:ss")} ");
                _ux.Backlog.WriteLine(message.RFQ.ToString());
            });

        }

        public static IActorRef Create(ActorSystem system)
        {
            var ux = new QuoteShared.UserInterface();
            var actor = system.ActorOf(Props.Create(() => new UserInterfaceActor(ux)), NAME);
            return actor;
        }

        public static string NAME = "UX";
        
    }
}
