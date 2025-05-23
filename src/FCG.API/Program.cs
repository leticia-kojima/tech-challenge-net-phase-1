using FCG.API.Endpoints;
using FCG.API.Middlewares;
using FCG.Infrastructure;
using FCG.Infrastructure._Common.Database;
using MongoDB.Driver;
using MySqlConnector;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Dependency Injection - DI

var services = builder.Services;
var configuration = builder.Configuration;

services.AddOpenApi()
    .AddDatabases(configuration)
    .AddRepositories()
    .AddInfrastructureServices();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

#region Endpoints

app.MapUserEndpoints();
app.MapGamesEndpoints();

#endregion

try
{
    // Database setup and migrations
    await app.Services.SetupDatabasesAsync(CancellationToken.None);
}
catch (Exception ex)
{
    // Create a clear error message based on the type of exception
    var errorMessage = new StringBuilder();
    errorMessage.AppendLine("Error starting application:");
    
    // Check if it's a database connection error and determine which database and error type
    if (IsConnectionException(ex, out string databaseType, out string errorType, out string detailedMessage))
    {
        if (errorType == "authentication")
        {
            errorMessage.AppendLine($"Authentication failed for {databaseType}. Please check your database credentials.");
            if (!string.IsNullOrEmpty(detailedMessage))
            {
                errorMessage.AppendLine($"Details: {detailedMessage}");
            }
        }
        else
        {
            errorMessage.AppendLine($"Connect Timeout expired in {databaseType}. Please check if the database service is running.");
        }
    }
    else
    {
        errorMessage.AppendLine("An unexpected error occurred during application startup.");
    }
    
    // Print the error to the console in a visible format
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(new string('=', 80));
    Console.WriteLine(errorMessage.ToString());
    Console.WriteLine(new string('=', 80));
    Console.ResetColor();
    
    // In development, display more details about the error
    if (app.Environment.IsDevelopment())
    {
        Console.WriteLine($"Exception type: {ex.GetType().Name}");
        Console.WriteLine($"Exception message: {ex.Message}");
        Console.WriteLine("Stack trace:");
        Console.WriteLine(ex.StackTrace);
    }
    
    // Exit with an error code
    Environment.Exit(1);
    return;
}

app.Run();

// Helper method to determine database type and error type from exception
static bool IsConnectionException(Exception ex, out string databaseType, out string errorType, out string detailedMessage)
{
    databaseType = "database";
    errorType = "connection";
    detailedMessage = string.Empty;
    
    var currentEx = ex;
    while (currentEx != null)
    {
        if (currentEx is MySqlException mysqlEx)
        {
            databaseType = "MySQL database";
            
            // Check for authentication error
            if (mysqlEx.Message.Contains("Access denied") || mysqlEx.Message.Contains("Authentication"))
            {
                errorType = "authentication";
                detailedMessage = mysqlEx.Message;
            }
            
            return true;
        }
        
        if (currentEx is MongoException mongoEx)
        {
            databaseType = "MongoDB database";
            
            // Check for authentication error
            if (mongoEx.Message.Contains("authenticate") || 
                mongoEx.Message.Contains("Authentication") ||
                mongoEx.Message.Contains("SCRAM-SHA-1") ||
                mongoEx.Message.Contains("Unable to authenticate"))
            {
                errorType = "authentication";
                detailedMessage = mongoEx.Message;
            }
            
            return true;
        }
        
        // Generic error detection
        if (currentEx.Message.Contains("Connect Timeout expired") || 
            currentEx.Message.Contains("Connection timed out") ||
            currentEx.Message.Contains("A connection attempt failed"))
        {
            // Determine database type from exception message or stack trace if possible
            if (currentEx.ToString().Contains("MySql") || ex.ToString().Contains("MySql"))
            {
                databaseType = "MySQL database";
            }
            else if (currentEx.ToString().Contains("Mongo") || ex.ToString().Contains("Mongo"))
            {
                databaseType = "MongoDB database";
            }
            
            // Check if it's an authentication error
            if (currentEx.Message.Contains("authenticate") ||
                currentEx.Message.Contains("Authentication") ||
                currentEx.Message.Contains("Access denied") ||
                currentEx.Message.Contains("Unable to authenticate") ||
                currentEx.Message.Contains("SCRAM-SHA-1") ||
                currentEx.ToString().Contains("authenticate") ||
                currentEx.ToString().Contains("Authentication"))
            {
                errorType = "authentication";
                detailedMessage = currentEx.Message;
            }
            
            return true;
        }
        
        currentEx = currentEx.InnerException;
    }
    
    return false;
}

