namespace CompanyEmployees;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults((IWebHostBuilder webBuilder) =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
