using System.Diagnostics;

namespace GameStore.Middlewares;

public class RequestTimingMiddleWare
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestTimingMiddleWare> logger;

    public RequestTimingMiddleWare(RequestDelegate next, ILogger<RequestTimingMiddleWare> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = new Stopwatch();

        try
        {
            stopWatch.Start();
            await next(context);
        }
        finally
        {
            stopWatch.Stop();

            var elapsedTimeMs = stopWatch.ElapsedMilliseconds;

            logger.LogInformation("Request {RequestMethod} {RequestPath} took {elapsedTimeMs}ms", context.Request.Method, context.Request.Path, elapsedTimeMs);
        }
    }

}