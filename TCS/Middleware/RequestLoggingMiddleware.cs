using System.Diagnostics;

namespace TCS.Middleware;

/// <summary>
/// IMiddleware-based request logger.
/// Because it implements IMiddleware, it is resolved from the DI container,
/// so we can inject ILogger (or any other registered service) into the constructor.
/// It logs every request that flows through the pipeline: method, path,
/// response status code and elapsed time.
/// </summary>
public class RequestLoggingMiddleware : IMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var start = Stopwatch.GetTimestamp();

        // --- Code before next() runs on the way IN ---
        _logger.LogInformation(
            "--> Request started: {Method} {Path}{Query}",
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString);

        // Pass the request to the next middleware / endpoint
        await next(context);

        // --- Code after next() runs on the way OUT (response already produced) ---
        var elapsedMs = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
        _logger.LogInformation(
            "<-- Request finished: {Method} {Path} -> {StatusCode} in {ElapsedMs:F1} ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            elapsedMs);
    }
}
