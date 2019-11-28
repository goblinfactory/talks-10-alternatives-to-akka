<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.App</NuGetReference>
  <NuGetReference>Microsoft.NETCore.App</NuGetReference>
  <NuGetReference>System.Interactive.Async</NuGetReference>
  <Namespace>Microsoft.AspNetCore</Namespace>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting</Namespace>
  <Namespace>Microsoft.AspNetCore.Mvc</Namespace>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

// convert the example to use GRPC  - https://www.stevejgordon.co.uk/creating-grpc-net-core-client-libraries
// YAY!

void Main()
{
	SelfHost.Run(@"http://*:5010");
}

public static class SelfHost
{
	public static void Run(string url) 
	{
		var args = new string[] { "" };
		CreateHostBuilder(url, args).Build().Run();
	}

	public static IWebHostBuilder CreateHostBuilder(string url, string[] args)
	{
		var host = WebHost.CreateDefaultBuilder(args)
		.UseUrls(url)
		.UseStartup<WebApi>();
		return host;
	}
}

public class WebApi
{
	public IConfiguration Configuration { get; }
	public WebApi(IConfiguration configuration) => Configuration = configuration;	
	public void ConfigureServices(IServiceCollection services) => services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);	
	public void Configure(IApplicationBuilder app, IHostingEnvironment env)
	{
		app.UseDeveloperExceptionPage();
		app.UseMvc();
	}
}

[Route("api/quotes")]
[ApiController]
public class QuoteController : ControllerBase
{
	[Route("q1")]
	[HttpGet]
	public IEnumerable<Quote> Get1()
	{
		var quotes = 10.Range().Select(i => new Quote(i * 100));
		return quotes;
	}


	[HttpGet("q2")]
	public IActionResult Get2()
	{
		var quotes = 10.Range().Select(i => new Quote(i * 100)).ToArray();
		return Ok(quotes);
	}

	[Route("q3")]
	[HttpGet]
	public async IAsyncEnumerable<Quote> Get3()
	{
		var r = new Random();
		foreach (int i in 10.Range())
		{
			yield return new Quote(i * 100);
			//await Task.Delay(r.Next() * 500);
		}
	}

}

public class Quote
{
	public Quote(decimal amount) => Amount = amount;
	public decimal Amount { get; }
}