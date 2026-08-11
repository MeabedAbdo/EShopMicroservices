using BuildingBlocks.Logging.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BuildingBlocks.Logging.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCompanyLogging(
          this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();

            return app;
        }
    }
}
