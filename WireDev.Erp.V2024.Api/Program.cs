using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WireDev.Erp.V1.Api.Context;

namespace WireDev.Erp.V1.Api
{
    public class Program
    {
        public static ILogger<Program>? Logger { get; set; }

        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            Logger = host.Services.GetRequiredService<ILogger<Program>>();
            Logger.LogInformation("Host created");
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    _ = webBuilder.UseStartup<Startup>();
                });
        }
    }
}