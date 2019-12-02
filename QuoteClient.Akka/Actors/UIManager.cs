using Akka.Actor;
using QuoteClient.Akka.Messages;
using QuoteShared.UX;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuoteClient.Akka.Commands
{
    public class UIManager : ReceiveActor
    {
        public class QuitPressed { }
        public class ToggleQuoteStreamPressed { }

        private readonly UserInterface _ux;
        private readonly IActorRef _quoteStream;
        private readonly Barrier _exitBarrier;

        public UIManager(UserInterface ux, IActorRef quoteStream, Barrier exitBarrier)
        {
            _ux = ux;
            _quoteStream = quoteStream;
            _exitBarrier = exitBarrier;
            
            Receive<string>(message =>
            {
                _ux.Status.WriteLine(message);
            });

            Receive<SpreadChanged>(message =>
            {
                _ux.Status.Update(message);
            });

            Receive<QuitPressed>(message =>
            {
                _ux.Status.WriteLine("quitting.");
                _exitBarrier.SignalAndWait();
            });

            Receive<BacklogCountChanged>(message =>
            {
                _ux.Status.Update(message);
            });

            Receive<RfqReceived>(message =>
            {
                _ux.Backlog.Write(ConsoleColor.Cyan, $"{DateTime.Now.ToString("hh:mm:ss")} ");
                _ux.Backlog.WriteLine(message.RFQ.ToString());
            });

            Receive<HandleKeys>(message =>
            {
                BlockUntilAnyKey();
            });

            Receive<Char>(key =>
            {
                switch (key) {
                    case 'q':
                        Self.Tell(new QuitPressed());
                        break;
                    case 'p':
                        Self.Tell(new ToggleQuoteStreamPressed());
                        break;
                }
            });
        }

        private void BlockUntilAnyKey(params char[] keys)
        {
            // this blocks a thread so we have to use a background task and PipeTo(Self)
            var task = Task.Run<char>(() =>
            {
                char key = 'a';
                while (true)
                {
                    while(!Console.KeyAvailable)
                    {
                        key = Console.ReadKey(true).KeyChar;
                        if (keys.Contains(key)) return key;
                    }
                }
            });
            task.PipeTo(Self);
        }

        public static IActorRef Create(ActorSystem system, IActorRef quoteStream, UserInterface userInterface, Barrier barrier)
        {
            if(barrier.ParticipantCount!=2 && barrier.ParticipantsRemaining!=2)
            {
                throw new ArgumentException("Barrier must be setup with 2 participants in order to use that as a means of blocking until user presses 'q'.");
            }
            var actor = system.ActorOf(Props.Create(() => new UIManager(userInterface, quoteStream, barrier)), NAME);
            return actor;
        }

        public static string NAME = "UIManager";
        
    }
}
