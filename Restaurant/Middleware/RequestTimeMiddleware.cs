using System.Diagnostics;

namespace Restaurant.Middleware
{
    public class RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger) : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopWatch = Stopwatch.StartNew();

            await next.Invoke(context);

            stopWatch.Stop();

            var elapsedSecondes = stopWatch.Elapsed.TotalSeconds;

            if (elapsedSecondes <= 4) 
            {
                string method = context.Request.Method;
                string path = context.Request.Path;
                logger.LogInformation("Request {Method} {Path} was executed in {time}", context, path, elapsedSecondes);
            }
        }
    }
}
