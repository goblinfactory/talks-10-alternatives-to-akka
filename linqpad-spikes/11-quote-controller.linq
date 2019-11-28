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

// quote controller

[Route("api/quotes")]
[ApiController]
public class QuoteController : ControllerBase
{
	[HttpGet]
	public async IAsyncEnumerable<Quote> Get()
	{
		var r = new Random();
		foreach (int i in 10.Range())
		{
			yield return new Quote(i * 100);
			await Task.Delay(r.Next() * 5000);
		}
	}
}

public class Quote
{
	public Quote(decimal amount) => Amount = amount;
	public decimal Amount { get; }
}