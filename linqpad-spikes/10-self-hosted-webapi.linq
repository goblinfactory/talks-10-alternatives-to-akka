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

// self hosted webapi that exposes a quote service that randombly blocks (simulating quotes coming in)
// reference nuget package 
// -----------------------
// Microsoft.AspNetCore
// Microsoft.AspNetCore.App
// System.Interactive.Async, (fix bug with IAsyncEnumerable from within Linqpad, Entity Framework bug afaik)

void Main()
{
	//Todo: add swagger
	// todo fluently configure the controller instead of wiring them ALL up!
	// try https when I sort this.
	// lets create a generic host that I can give a simple lambda
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


// TODO: Update SelfHost to allow for fluently configuring routes
[Route("api")]
[ApiController]
public class QuoteController : ControllerBase
{
	private static Random rnd = new Random();
	private static Dictionary<Guid, Quote> _quotes = new Dictionary<Guid, Quote>();
	private static int _quote = 1;
	private static int _slowing = 100; // used to simulate a server slowing down.


	[Route("quotes/status")]
	[HttpGet]
	public ActionResult<string> Status()
	{
		return Ok($"Hello. server time is {DateTime.UtcNow.ToShortDateString()}");
	}

	[Route("quotes/{id}")]
	[HttpGet]
	public ActionResult<Quote> GetQuote(Guid id)
	{
		if(!_quotes.ContainsKey(id)) return NotFound();
		return Ok(_quotes[id]);
	}

	[Route("/quotes")]
	[HttpPost]
	public async Task<ActionResult<QuoteRequest>> GetQuote(QuoteRequest info)
	{
		var id = Guid.NewGuid();
		var quote = new Quote(_quote++ * 100, info);

		if(info.RegNo.Contains("429")) {

		}
		
		await Task.Delay(rnd.Next(1000));
		_quotes.Add(id, quote);
		return Ok(quote);
	}
}

public class QuoteRequest
{
	public QuoteRequest(string driver, int age, string regNo, string car) {
		Driver = driver; Age = age; RegNo = regNo; Car = car;
	}
	public string Driver { get; }
	public int Age { get; }
	public string RegNo { get; }
	public string Car { get; }
}

public class Quote
{
	public Quote(decimal amount, QuoteRequest info)
	{
		Amount = amount;
		Info = info;
	}
	public decimal Amount { get; }
	public QuoteRequest Info { get; }
}

// references : 
// action return types - https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.0
// throttling webapi endpoints in asp.net core - https://codeburst.io/rate-limiting-api-endpoints-in-asp-net-core-926e31428017