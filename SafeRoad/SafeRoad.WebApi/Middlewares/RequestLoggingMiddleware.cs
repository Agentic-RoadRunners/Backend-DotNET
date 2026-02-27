using System.Diagnostics;

namespace SafeRoad.WebApi.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var method = context.Request.Method;
        var path = context.Request.Path;
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        _logger.LogInformation("→ {Method} {Path} | IP: {IP}", method, path, ip);

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation("← {Method} {Path} | Status: {StatusCode} | {ElapsedMs}ms",
            method, path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}
