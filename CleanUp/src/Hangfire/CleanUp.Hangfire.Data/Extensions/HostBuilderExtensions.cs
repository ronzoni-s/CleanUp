using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CleanUp.Hangfire.Data
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseCustomSerilog(this IHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                //.AddJsonFile("appsettings.Development.json")
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            SerilogHostBuilderExtensions.UseSerilog(builder);

            return builder;
        }
    }
}
