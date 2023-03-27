using CompanyEmployees.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyEmployees.Client;

class Program
{
    static async Task Main(string[] args)
	{
		var services = new ServiceCollection();

		ConfigureServices(services);

		var provider = services.BuildServiceProvider();

		try
		{
			await provider.GetRequiredService<IHttpClientServiceImplementation>().ExecuteAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Something went wrong: {ex}");
		}
	}

	private static void ConfigureServices(IServiceCollection services) =>
		services.AddScoped<IHttpClientServiceImplementation, HttpClientPatchService>();
}
