using System.Net;
using System.Text.Json;

namespace FCG.API.Middlewares;

public class UnauthorizedMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UnauthorizedMiddleware> _logger;

    public UnauthorizedMiddleware(RequestDelegate next, ILogger<UnauthorizedMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Unauthorized request intercepted.");

                var result = JsonSerializer.Serialize(new
                {
                    error = "Unauthorized. Token is missing, invalid, or expired."
                });

                context.Response.ContentType = "application/json";
                context.Response.ContentLength = result.Length;

                memoryStream.SetLength(0);
                await memoryStream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(result));
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                _logger.LogWarning("Forbidden request intercepted.");

                var result = JsonSerializer.Serialize(new
                {
                    error = "Forbidden. You do not have permission to access this resource."
                });

                context.Response.ContentType = "application/json";
                context.Response.ContentLength = result.Length;

                memoryStream.SetLength(0);
                await memoryStream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(result));
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
    public static class UnauthorizedMiddlewareExtensions
    {
        public static IApplicationBuilder UseUnauthorizedMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UnauthorizedMiddleware>();
        }
    }
}

