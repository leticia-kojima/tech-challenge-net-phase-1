using FCG.Domain._Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace FCG.API.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next, 
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred.";
        
        // Log the exception
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        switch (exception)
        {
            case FCGDuplicateException duplicateEx:
                statusCode = HttpStatusCode.Conflict; // 409 Conflict
                message = duplicateEx.Message;
                break;
                
            case FCGNotFoundException notFoundEx:
                statusCode = HttpStatusCode.NotFound; // 404 Not Found
                message = notFoundEx.Message;
                break;
                
            case FCGValidationException validationEx:
                statusCode = HttpStatusCode.BadRequest; // 400 Bad Request
                message = validationEx.Message;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message
        };
        
        if (_environment.IsDevelopment())
        {
            response.ExceptionType = exception.GetType().Name;
            response.ExceptionMessage = exception.Message;
            response.StackTrace = exception.StackTrace;
        }

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}
