<Query Kind="Statements">
  <NuGetReference>Bogus</NuGetReference>
  <Namespace>Bogus</Namespace>
  <Namespace>Bogus.Extensions.UnitedKingdom</Namespace>
  <Namespace>Bogus.DataSets</Namespace>
</Query>


// connect to fake data services and generate a nice json file full of can quote applications


// we want to process 30 car quotes,
// average latency across a panel of 20 providers = 100 to 3000, i.e. 1450ms per quote 
// = a theoretical minimum of 45 seconds, with all panelist responding.

// our goal is to get as many providers to quote as possible
// and maximize throughput (quotes) per second, by responding as soon as we have X quotes.
// we also don't want to blow up in smoke with the very first quote.


// CAN WE DO THAT?
// =========================================
// What happens with the simple approach?
// how to do this the Akka way?
// how to do this alternative .net libraries?

// we're going to have 300 applications
// we want
// 2 services to go down permanently with BOOM
// every 

var r = new Random();
10.Range().Select(i => {
	var f = new Faker("en");
	var p = f.Person;
	return new
	{
		Driver = $"{p.FullName}",
		Age = r.Next(40) + 18,
		RegNo = Bogus.Extensions.UnitedKingdom
			.RegistrationPlateExtensionsForGreatBritain
			.GbRegistrationPlate(new Vehicle(), new DateTime(2002, 1, 1), new DateTime(2019, 1, 1))
	};
}).Dump();