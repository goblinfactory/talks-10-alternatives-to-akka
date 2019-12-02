using Akka.Actor;
using QuoteClient.Akka.Commands;

namespace QuoteClient.Akka
{
    public partial class ProgramActor : ReceiveActor
    {
        private readonly IActorRef _ux;
        private readonly IActorRef _monitor;

        public ProgramActor(IActorRef ux, IActorRef monitor)
        {
            _ux = ux;
            _monitor = monitor;

            Receive<RunProgram>(cmd =>
            {
                ux.Tell("Starting, waiting for upstream services;");
                ux.Tell("pretending they are all running");
                _monitor.Tell(new StartMonitoring());
                ux.Tell("press (p) pause or resume, (q) to quit.");
                ux.Tell(new HandleKeys());
            });

        }
        public static IActorRef Create(ActorSystem system, IActorRef ux, IActorRef upstreamMonitor)
        {
            return system.ActorOf(Props.Create(() => new ProgramActor(ux, upstreamMonitor)), NAME);
        }
        public static string NAME = "QuoteClient";
    }
}
