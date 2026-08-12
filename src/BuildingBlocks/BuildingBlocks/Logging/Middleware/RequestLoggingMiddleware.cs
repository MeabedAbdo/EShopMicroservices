using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Logging.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var start = DateTime.UtcNow;

            await _next(context);

            var duration =
                DateTime.UtcNow.Subtract(start).TotalMilliseconds;

            _logger.LogInformation(
                "Request {Method} {Path} completed in {Duration} ms with status {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                duration,
                context.Response.StatusCode);
        }
    }
}
