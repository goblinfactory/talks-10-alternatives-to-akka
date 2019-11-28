<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.Configuration</NuGetReference>
  <NuGetReference>Microsoft.Extensions.FileProviders.Abstractions</NuGetReference>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>System.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.FileProviders</Namespace>
  <Namespace>Microsoft.Extensions.Primitives</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

// looks like I need to step aaway from linqpad, just getting to damned messy.



public class Program
{
		  
	static public IConfiguration Configuration { get; set; }
	
	public static void Main(string[] args = null)
	{

		var memoryFileProvider = new InMemoryFileProvider("{ name:'Fred', age:10 }");
		var configuration = new ConfigurationBuilder()
			.AddJsonFile(memoryFileProvider, "appsettings.json", false, false).Build();

		var builder = new ConfigurationBuilder();
		//builder.AddConfiguration(
			
	//	
	//	builder.AddInMemoryCollection(DefaultConfigurationStrings);
	//	Configuration = builder.Build();
	//	
	//	Console.WriteLine($"Hello {Configuration["Profile:UserName"]}");
	//	
	//	
	//
	//	var window = Configuration.GetSection("AppConfiguration:MainWindow")
	}
}
public class MyWindow
{
	public int Height { get; set; }
	public int Width { get; set; }
	public int Top { get; set; }
	public int Left { get; set; }

}



// from -> https://stackoverflow.com/questions/44807836/loading-startup-configuration-from-json-located-in-memory

public class InMemoryFileProvider : IFileProvider
{
	private class InMemoryFile : IFileInfo
	{
		private readonly byte[] _data;
		public InMemoryFile(string json) => _data = Encoding.UTF8.GetBytes(json);
		public Stream CreateReadStream() => new MemoryStream(_data);
		public bool Exists { get; } = true;
		public long Length => _data.Length;
		public string PhysicalPath { get; } = string.Empty;
		public string Name { get; } = string.Empty;
		public DateTimeOffset LastModified { get; } = DateTimeOffset.UtcNow;
		public bool IsDirectory { get; } = false;
	}

	private readonly IFileInfo _fileInfo;
	public InMemoryFileProvider(string json) => _fileInfo = new InMemoryFile(json);
	public IFileInfo GetFileInfo(string _) => _fileInfo;
	public IDirectoryContents GetDirectoryContents(string _) => null;
	public IChangeToken Watch(string _) => NullChangeToken.Singleton;
}

// see also;
// https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited
