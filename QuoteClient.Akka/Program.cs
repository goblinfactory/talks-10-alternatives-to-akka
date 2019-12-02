using Akka.Actor;
using Newtonsoft.Json;
using QuoteClient.Akka.Commands;
using QuoteShared;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace QuoteClient.Akka
{
    class Program
    {
        static Barrier Finished = new Barrier(2);
        static async Task Main(string[] args)
        {
            using (var system = ActorSystem.Create("GoblinfactoryInsuranceQuotes"))
            {
                Thread.Sleep(500);
               
                var providers = JsonConvert.DeserializeObject<InsuranceProvider[]>(File.ReadAllText("panel.json"));
                var program = Startup.ConfigureServices(system, providers, Finished);
                var run = new RunProgram(providers);
                program.Tell(run);

                Finished.SignalAndWait();
                await system.WhenTerminated;
            }
        }

    }



    // BacklogManager
    //--------------

    // UpstreamMonitor
    // -------------
    // checks the status of ALL the upstream services and notifies the quote manager when enough services are ready.
    // if a services goes down, then tells the quoteManager to change status to OFFLINE

    // QuoteFetcher
    // -------------
    // responsible for getting a single quote from a provider 

    // QuoteTracker
    // ------------
    // tracks all quotes that come in for a specific RFQ 
    // as soon as we have 10 quotes in, tell the quote manager we dont need any more quotes.
    // if all quotes are in or have failed and we don't have 10
    // tell the quote manager when the job is done, or has failed.
    // tell the quotetracker our % progress

    // UserInterfaceActor
    // ------------------
    // does UI stuff

    // TIME PERMITTING configure Akka to deploy the UI remotely.

}
