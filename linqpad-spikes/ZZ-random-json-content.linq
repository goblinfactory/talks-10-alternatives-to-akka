<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

static async Task Main()
{
	var client = new HttpClient();

	var resp = await client.PostAsync("http://localhost:5024/api/car-insurance/quotes", new JsonContent(new
	{
		Driver = "Fred Smith",
		age = 18,
		RegNo = "ABC  1234",
		Car = "BMW Z3 Sports convertable"
	}));
	
	var json = await resp.Content.ReadAsStringAsync();
	json.Dump();
	
}

// Define other methods, classes and namespaces here
public class JsonContent : StringContent
{
	public JsonContent(object obj) :
		base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
	{ }
}

