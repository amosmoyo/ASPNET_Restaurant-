using Microsoft.AspNetCore.Http;

namespace Restaurant.Middleware
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var erroeResponse = new
                {
                    Status = 500,
                    Message = ex.Message,
                };

                await context.Response.WriteAsJsonAsync(erroeResponse);

            }
        }
    }
}
