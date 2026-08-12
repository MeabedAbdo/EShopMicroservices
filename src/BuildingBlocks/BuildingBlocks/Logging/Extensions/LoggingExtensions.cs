using BuildingBlocks.Logging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.MSSqlServer;

namespace BuildingBlocks.Logging.Extensions
{
      public static class LoggingExtensions
        {
            public static IServiceCollection AddSharedLogging(
                this IServiceCollection services,
                LoggingOptions options)
            {
                var loggerConfig = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId();

                if (options.EnableConsole)
                {
                    loggerConfig.WriteTo.Console();
                }

                if (options.EnableFile)
                {
                    loggerConfig.WriteTo.File(
                        options.FilePath,
                        rollingInterval: RollingInterval.Day);
                }

                if (options.EnableSqlServer &&
                    !string.IsNullOrWhiteSpace(options.SqlConnectionString))
                {
                    loggerConfig.WriteTo.MSSqlServer(
                        connectionString: options.SqlConnectionString,
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            TableName = options.TableName,
                            AutoCreateSqlTable = true
                        });
                }
                if (options.EnableSeq && !string.IsNullOrWhiteSpace(options.SeqURL))
                {
                    loggerConfig.WriteTo.Seq(options.SeqURL);
                }
                Log.Logger = loggerConfig.CreateLogger();

                services.AddLogging(builder =>
                {
                    builder.AddSerilog();
                });

                return services;
            }
        }

    
}
