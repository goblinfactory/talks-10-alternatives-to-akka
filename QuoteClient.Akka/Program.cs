using Akka.Actor;
using Newtonsoft.Json;
using QuoteClient.Akka.Commands;
using QuoteShared;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace QuoteClient.Akka
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var system = ActorSystem.Create("GoblinfactoryInsuranceQuotes"))
            {
                var finished = new Barrier(2, _ => system.Terminate());
                Thread.Sleep(500);

                var providers = JsonConvert.DeserializeObject<InsuranceProvider[]>(File.ReadAllText("panel.json"));
                var program = Startup.ConfigureServices(system, providers, finished);
                program.Tell(new RunProgram(providers));

                finished.SignalAndWait();
                await system.WhenTerminated;

                Console.Clear();
                Console.WriteLine("finished.");
            }
        }
    }
}
