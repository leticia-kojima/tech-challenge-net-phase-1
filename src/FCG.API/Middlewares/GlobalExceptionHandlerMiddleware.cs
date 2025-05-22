using FCG.Domain._Common.Exceptions;
using MongoDB.Driver;
using MySqlConnector;
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

        // Check for database connection exceptions (including inner exceptions)
        if (IsDatabaseConnectionException(exception, out var friendlyMessage))
        {
            statusCode = HttpStatusCode.ServiceUnavailable; // 503 Service Unavailable
            message = friendlyMessage;
        }
        else
        {
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

    private bool IsDatabaseConnectionException(Exception ex, out string friendlyMessage)
    {
        // Check current exception and all inner exceptions
        var currentEx = ex;
        while (currentEx != null)
        {
            if (currentEx is MySqlException mysqlEx)
            {
                // Check for different MySQL error types
                if (mysqlEx.Message.Contains("Access denied") || mysqlEx.Message.Contains("Authentication"))
                {
                    friendlyMessage = "Authentication failed for MySQL database. Please check your database credentials.";
                }
                else if (mysqlEx.Message.Contains("Connect Timeout") || mysqlEx.Message.Contains("Connection timed out"))
                {
                    friendlyMessage = "Connect Timeout expired in MySQL database. Please check if the database service is running.";
                }
                else
                {
                    friendlyMessage = "Error connecting to MySQL database. " + mysqlEx.Message;
                }
                
                // Log detailed information about the exception
                _logger.LogError("MySQL Connection Error: {Message}", currentEx.Message);
                return true;
            }
            
            if (currentEx is MongoException mongoEx)
            {
                // Check for different MongoDB error types
                if (mongoEx.Message.Contains("authenticate") || 
                    mongoEx.Message.Contains("Authentication") ||
                    mongoEx.Message.Contains("SCRAM-SHA-1") ||
                    mongoEx.Message.Contains("Unable to authenticate"))
                {
                    friendlyMessage = "Authentication failed for MongoDB database. Please check your database credentials.";
                }
                else if (mongoEx.Message.Contains("Timeout") || mongoEx.Message.Contains("timed out"))
                {
                    friendlyMessage = "Connect Timeout expired in MongoDB database. Please check if the database service is running.";
                }
                else
                {
                    friendlyMessage = "Error connecting to MongoDB database. " + mongoEx.Message;
                }
                
                // Log detailed information about the exception
                _logger.LogError("MongoDB Connection Error: {Message}", currentEx.Message);
                return true;
            }
            
            // Special case for timeout errors or general connection errors
            if (currentEx.Message.Contains("Connect Timeout expired") || 
                currentEx.Message.Contains("Connection timed out") ||
                currentEx.Message.Contains("A connection attempt failed"))
            {
                // Determine database type from message or stack trace if possible
                string dbType = "database";
                string errorType = "connection timeout";
                
                if (currentEx.Message.Contains("MySql") || 
                    currentEx.ToString().Contains("MySql") || 
                    ex.ToString().Contains("MySql"))
                {
                    dbType = "MySQL database";
                }
                else if (currentEx.Message.Contains("Mongo") || 
                         currentEx.ToString().Contains("Mongo") || 
                         ex.ToString().Contains("Mongo"))
                {
                    dbType = "MongoDB database";
                }
                
                // Check for authentication errors
                if (currentEx.Message.Contains("authenticate") ||
                    currentEx.Message.Contains("Authentication") ||
                    currentEx.Message.Contains("Access denied") ||
                    currentEx.Message.Contains("Unable to authenticate") ||
                    currentEx.Message.Contains("SCRAM-SHA-1") ||
                    currentEx.ToString().Contains("authenticate") ||
                    currentEx.ToString().Contains("Authentication"))
                {
                    errorType = "authentication";
                    friendlyMessage = $"Authentication failed for {dbType}. Please check your database credentials.";
                }
                else
                {
                    friendlyMessage = $"Connect Timeout expired in {dbType}. Please check if the database service is running.";
                }
                
                // Log detailed information about the exception
                _logger.LogError("Database {ErrorType} Error: {Message}", errorType, currentEx.Message);
                return true;
            }
            
            currentEx = currentEx.InnerException;
        }
        
        friendlyMessage = string.Empty;
        return false;
    }
}
