using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace Eshop.Logging
{
    public static class LoggingExtensions
    {
        public static WebApplicationBuilder AddSharedLogging(this WebApplicationBuilder builder, string applicationName)
        {
            // Bind settings from the microservice's appsettings.json
            var configuration = builder.Configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Information()
                // Suppress noisy framework logs
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                // Automatically capture request IDs and trace properties
                .Enrich.FromLogContext()
                // Inject the service name globally to allow easy filtering inside Seq
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .WriteTo.Console()
                // Safe fallback configuration to local dev if settings are missing
                .WriteTo.Seq(configuration["Serilog:SeqServerUrl"] ?? "http://localhost:5341")
                .CreateLogger();

            // Register Serilog as the primary logging provider
            builder.Host.UseSerilog();

            return builder;
        }

    }
}
