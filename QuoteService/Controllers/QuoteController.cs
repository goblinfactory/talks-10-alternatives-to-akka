using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuoteService.Internal;
using QuoteService.Models;

namespace QuoteService.Controllers
{
	[Route("api")]
	[ApiController]
	public class QuoteController : ControllerBase
	{
		private static Random _rnd = new Random();
		private static Dictionary<Guid, Quote> _quotes = new Dictionary<Guid, Quote>();
		private static readonly int _speed = 1000;
		private readonly IQuoterConfig _quoterConfig;
		private Availability _availability;

		public QuoteController(IQuoterConfig quoteConfig)
		{
			_quoterConfig = quoteConfig;
			_availability = Enum.Parse<Availability>(quoteConfig.Availability);
		}

		[Route("car-insurance/status")]
		[HttpGet]
		public string Status()
		{
			return $"{_quoterConfig.Name}. server time is {DateTime.UtcNow}. PauseMsBeforeResponse:{_quoterConfig.Speed}";
		}

		[Route("car-insurance/quotes")]
		[HttpPost]
		public async Task<ActionResult<CarInsuranceRequest>> GetCarInsuranceQuote(CarInsuranceRequest info)
		{
			// for this demo, we're only going to be processing approximately 30 quotes each per Insurer on the panel.
			var chance = new Chance(_availability);

			// check if we're being throttled
			// ------------------------------
			if(BackoffSimulator.IsCurrentlyLimiting<CarInsuranceRequest>(DateTime.Now))
			{
				Response.Headers.Add("Retry-After", BackoffSimulator.HowLong<CarInsuranceRequest>().ToString());
				return StatusCode((int)HttpStatusCode.TooManyRequests);
			}

			// `429` Simulate Too many requests with backoff
			// --------------------------------------------
			if (chance.ShouldBackOff())
			{
				// poor mans rate limiter, simulate an upstream partner service, e.g. a panel member providing car quotes. (insurance company X)
				BackoffSimulator.TakeServiceOfflineUntil<CarInsuranceRequest>(DateTime.Now.AddSeconds(20));
				Response.Headers.Add("Retry-After", "20");
				return StatusCode((int)HttpStatusCode.TooManyRequests);
			}

			// not doing this for now.
			//// car reg contains `404` 
			//// ----------------------
			//// should return 404 `Not found`, a retry should never succeed.
			//if (info.RegNo.Contains("-404-"))
			//{
			//	return NotFound();
			//}

			// car quote service goes `BOOM`!
			// ----------------------
			if (chance.GoBoom())
			{
				throw new Exception($"BOOM, {_quoterConfig.Name} service has died! and will not be coming back online for a few hours, perhaps 1 or even 2 days!");
			}

			// DO THE ACTUAL FAKE QUOTE
			// ----------------------
			var id = Guid.NewGuid();

			var quote = new Quote(id, ((decimal)_rnd.Next(1000)) / 10M, _quoterConfig.Name, info);
			await Task.Delay(_rnd.Next(_speed));

			_quotes.Add(id, quote);
			return Ok(quote);
		}

		[Route("car-insurance/quotes/{id}")]
		[HttpGet]
		public ActionResult<Quote> GetQuote1(Guid id)
		{
			if (!_quotes.ContainsKey(id)) return NotFound();
			return Ok(_quotes[id]);
		}

	}
}

// Stretch goals...
// fix the model binding to immutable clases problem 
// https://dejanstojanovic.net/aspnet/2019/september/mixed-model-binding-in-aspnet-core-using-custom-model-binders/
