using FCG.API.Endpoints;
using FCG.API.Middlewares;
using FCG.Infrastructure;
using FCG.Infrastructure._Common.Auth;
using FCG.Infrastructure._Common.Database;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Dependency Injection - DI
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer()
    .AddFcgApiSwagger()
    .AddOpenApi()
    .AddDatabases(configuration)
    .AddRepositories()
    .AddInfrastructureServices();

services.ConfigureSettings(configuration)
    .ConfigureAuthentication(configuration)
    .ConfigureAuthorization();

#endregion

#region Serilog

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FCG API"));

    app.MapOpenApi();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>()
    .UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

#region Endpoints

app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapCatalogEndpoints();
app.MapGamesEndpoints();

#endregion

await app.Services
    .SetupDatabasesAsync(CancellationToken.None);

app.Run();
