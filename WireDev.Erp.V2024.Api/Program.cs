using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WireDev.Erp.V2024.Api.Context;

namespace WireDev.Erp.V2024.Api
{
    public class Program
    {
        public static ILogger<Program>? Logger { get; set; }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
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