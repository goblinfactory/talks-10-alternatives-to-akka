<Query Kind="Program">
  <NuGetReference>AspNetCoreRateLimit</NuGetReference>
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
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>AspNetCoreRateLimit</Namespace>
</Query>

// is everything we need here? -> https://codeburst.io/rate-limiting-api-endpoints-in-asp-net-core-926e31428017
// or-> https://github.com/changhuixu/dotnetlabs/tree/master/CSharpLabs/SemaphoreSlimThrottle/ThrottledWebApi
// see also my in memory configuration provider.


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

// OK I am going to need an in memory app Configuration setting provider.

public class WebApi
{
	public IConfiguration Configuration { get; }
	public WebApi(IConfiguration configuration) => Configuration = configuration;
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		DemoRateLimiter.AddRateLimiting(services);
	}
	
	public void Configure(IApplicationBuilder app, IHostingEnvironment env)
	{
		app.UseDeveloperExceptionPage();
		app.UseClientRateLimiting();  // <--- this has to be added as well! I keep missing this. Can't configure this in one swoop.
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

	public QuoteController(IOptions<IpRateLimitOptions> options, IIpPolicyStore store) 
	{
		var limiter = new DemoRateLimiter(options, store);
		limiter.AddRateLimit("*:/api/status", 2, LimitPeriod.PerMinute);
	}

	[Route("status")]
	[HttpGet]
	public ActionResult<string> Status()
	{
		return Ok($"Hello. server time is {DateTime.UtcNow}");
	}
	
	[Route("quotes/{id}")]
	[HttpGet]
	public ActionResult<Quote> GetQuote1(Guid id)
	{
		if(!_quotes.ContainsKey(id)) return NotFound();
		return Ok(_quotes[id]);
	}

	[Route("/quotes")]
	[HttpPost]
	public async Task<ActionResult<QuoteRequest>> GetQuote2(QuoteRequest info)
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



public enum LimitPeriod
{
	PerSecond, PerMinute
}

public class DemoRateLimiter
{
	public static void AddRateLimiting(IServiceCollection services)
	{

		services.AddOptions();
		services.AddMemoryCache();
		services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
		services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
		services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
		services.AddHttpContextAccessor();
	}

	private string _IP;
	private IpRateLimitOptions _options;
	private IIpPolicyStore _store;

	public DemoRateLimiter(IOptions<IpRateLimitOptions> rateLimitOptions, IIpPolicyStore store, string IP = "localhost")
	{
		_options = rateLimitOptions.Value;
		_store = store;
		_IP = IP;
	}

	/// <summary>configure the rate limits for an endpoint, e.g. 'api/testupdate'</summary>
	public void AddRateLimit(string endpoint, int limit = 2, LimitPeriod per = LimitPeriod.PerMinute)
	{
		//var pol = _store.GetAsync(_options.IpPolicyPrefix).Result;
		var pol = new IpRateLimitPolicies();
		var pers = per.ToString();
		var policy = new IpRateLimitPolicy
		{
			Ip = _IP,
			Rules = new List<RateLimitRule>(new RateLimitRule[] {
				new RateLimitRule {
					Endpoint = $"{endpoint}",
					Limit = limit,
					Period = pers
				}
			})
		};
		pol.IpRules.Add(policy);
		
		// seed or set?
		_store.SetAsync(_options.IpPolicyPrefix, pol).Wait();
		_store.SeedAsync().Wait();
	}
}


public static class PeriodExtensions
{
	public static string ToString(this LimitPeriod src)
	{
		return src == LimitPeriod.PerSecond ? "1m" : "1s";
	}
}