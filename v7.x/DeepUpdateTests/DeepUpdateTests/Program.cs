using Microsoft.AspNet.OData.Formatter.Deserialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OData.Edm;

namespace DeepUpdateTests;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
