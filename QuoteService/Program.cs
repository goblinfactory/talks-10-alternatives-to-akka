using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuoteShared;
using System.Collections.Generic;

namespace QuoteService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Run(args, new InsuranceProvider("test-company", 100, 5100, Availability.Excellent.ToString()));
        }


        public static void Run(string[] args, InsuranceProvider settings)
        {
            new ServiceCollection()
                .AddSingleton<IInsuranceProvider>(settings)
                .BuildServiceProvider();

            CreateHostBuilder(args, settings.Port, settings)
                .Build()
                .Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args, int port, InsuranceProvider settings) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(sc =>
                {
                    sc.AddSingleton<IInsuranceProvider>(settings);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls($"http://*:{port}")
                    .UseStartup<Startup>();
                });
    }
}