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
  <Namespace>AspNetCoreRateLimit</Namespace>
  <Namespace>System.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
</Query>

// ADD RATE LIMITING TO EXISTING API

public void Main() { 
	// press F5 to make sure it all compiles.
}

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
		//services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting")); do this fluently see below instead of via json settings, so that I can have a simple demo.
		services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
		services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
		services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
		services.AddHttpContextAccessor();
	}
	
	private string _IP;
	private IpRateLimitOptions _options;
	private IIpPolicyStore _store;
	
	public DemoRateLimiter(IOptions<IpRateLimitOptions> rateLimitOptions, IIpPolicyStore store, string IP = "127.0.0.1")
	{
		_options = rateLimitOptions.Value;
		_store = store;
		_IP = IP;
	}

	/// <summary>configure the rate limits for an endpoint, e.g. 'api/testupdate'</summary>
	public void ConfigureRateLimit(string endpoint, int limit = 2, LimitPeriod per = LimitPeriod.PerSecond)
	{
		var pol = _store.GetAsync(_options.IpPolicyPrefix).Result;

		pol.IpRules.Add(new IpRateLimitPolicy
		{
			Ip = _IP,
			Rules = new List<RateLimitRule>(new RateLimitRule[] {
				new RateLimitRule {
					Endpoint = $"*:/{endpoint}",
					Limit = limit,
					Period = per.ToString() }
			})
		});
		_store.SetAsync(_options.IpPolicyPrefix, pol);
	}
}


public static class PeriodExtensions 
{
	public static string ToString(this LimitPeriod src) {
		return src == LimitPeriod.PerSecond ? "1m" : "1s";
	}
}

// references : https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#update-rate-limits-at-runtime