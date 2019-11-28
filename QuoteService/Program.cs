using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuoteService.Models;
using System.Collections.Generic;

namespace QuoteService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Run(args, new QuoterConfig("test-company", 100, 5100, Availability.Excellent.ToString()));
        }


        public static void Run(string[] args, QuoterConfig settings)
        {
            new ServiceCollection()
                .AddSingleton<IQuoterConfig>(settings)
                .BuildServiceProvider();

            CreateHostBuilder(args, settings.Port, settings)
                .Build()
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args, int port, QuoterConfig settings) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(sc =>
                {
                    sc.AddSingleton<IQuoterConfig>(settings);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls($"http://*:{port}")
                    .UseStartup<Startup>();
                });
    }
}