using Newtonsoft.Json;
using QuoteShared;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace QuotePanel
{
    class Program
    {
        static void Main(string[] args)
        {
            var panel = JsonConvert.DeserializeObject<InsuranceProvider[]>(File.ReadAllText("panel.json"));

            var servers = new List<Task>();
            foreach(var quoter in panel)
            {
                var server = Task.Run(() => QuoteService.Program.Run(args, quoter));
                servers.Add(server);
            }
            
            Task.WaitAll(servers.ToArray());
        }
    }
}