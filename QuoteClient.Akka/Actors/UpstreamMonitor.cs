using Akka.Actor;
using Newtonsoft.Json;
using System.IO;
using QuoteShared;
using QuoteClient.Akka.Commands;
using System.Threading.Tasks;
using Konsole;
using System.Threading;
using System;

namespace QuoteClient.Akka
{
    /// <summary>
    /// Upstream monitor that will monitor (polling) all upstream providers
    /// and let the Backlog Manager know about any changes.
    /// </summary>
    public class UpstreamMonitor : ReceiveActor
    {
        private InsuranceProvider[] _insurers;
        private readonly IActorRef _quoteStream;
        private readonly IConsole _console;

        public UpstreamMonitor(IActorRef quoteStream, IConsole console, InsuranceProvider[] insurers)
        {
            _insurers = insurers;
            _quoteStream = quoteStream;
            _console = console;

            Receive<StartMonitoring>(message => {
                _console.WriteLine("starting monitoring");
                Thread.Sleep(2000);
                foreach (var panel in _insurers)
                {
                    // skipping testing the actual endpoints for now for tonights demo
                    // todo, use Konsole to printAt the live (polled) status of each service
                    // use an actor for each, taking the row position (scrollable)
                    _console.Write($"{panel.Name,-30}");
                    Thread.Sleep(50);
                    _console.WriteLine(ConsoleColor.Green, "(200)");
                    Thread.Sleep(50);
                }
                _console.WriteLine("telling quotestream to start");
                _quoteStream.Tell(new StartStreamingTestData());
            });
        }

        public static IActorRef Create(ActorSystem system, IActorRef quoteStream, IConsole console, InsuranceProvider[] insurers)
        {
            var actor = system.ActorOf(Props.Create(() => new UpstreamMonitor(quoteStream, console, insurers)), NAME);
            return actor;
        }

        public static string NAME = "UpstreamMonitor";
    }
}


    // UpstreamMonitor
    // -------------
    // checks the status of ALL the upstream services and notifies the quote manager when enough services are ready.
    // if a services goes down, then tells the quoteManager to change status to OFFLINE
