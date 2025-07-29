using FCG.API.Endpoints;
using FCG.API.Middlewares;
using FCG.Infrastructure;
using FCG.Infrastructure._Common.Auth;
using FCG.Infrastructure._Common.Database;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Dependency Injection - DI

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
    .Enrich.FromLogContext()
    .WriteTo.NewRelicLogs(
        licenseKey: builder.Configuration["NewRelic:LicenseKey"],
        applicationName: builder.Configuration["NewRelic:ApplicationName"])
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
