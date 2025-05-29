using FCG.Application._Common.Extensions;
using FCG.Domain._Common.Abstract;
using FCG.Domain._Common.Exceptions;
using System.Net;

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
        var errorResponse = null as ErrorResponse;

        try
        {
            await _next(context);
        }
        catch (FCGDuplicateException duplicateException)
        {
            errorResponse = new ErrorResponse(duplicateException);
        }
        catch (FCGNotFoundException notFoundException)
        {
            errorResponse = new ErrorResponse(notFoundException);
        }
        catch (FCGValidationException validationException)
        {
            errorResponse = new ErrorResponse(validationException);
        }
        catch (Exception exception)
        {
            errorResponse = new ErrorResponse(
                HttpStatusCode.InternalServerError,
                _environment.IsDevelopment()
                    ? exception.GetFullMessageString()
                    : "An unexpected error occurred."
            );
            _logger.LogError(
                exception,
                "Unhandled exception occurred. Path: {Path}, Method: {Method}, Error: {ErrorMessage}",
                context.Request.Path,
                context.Request.Method,
                exception.GetFullMessageString()
            );
        }

        if (errorResponse is null) return;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)errorResponse.StatusCode;

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}
