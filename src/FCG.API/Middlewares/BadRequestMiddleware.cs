using System.Net;
using System.Text.Json;

namespace FCG.API.Middlewares;

public class BadRequestMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BadRequestMiddleware> _logger;

    public BadRequestMiddleware(RequestDelegate next, ILogger<BadRequestMiddleware> logger)
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

            if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                _logger.LogWarning("Requisição inválida (400) interceptada.");

                var result = JsonSerializer.Serialize(new
                {
                    error = "Requisição inválida.",
                    detail = "Os dados enviados na requisição estão incorretos ou incompletos."
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
}
