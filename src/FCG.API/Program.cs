using FCG.API.Endpoints;
using FCG.Infrastructure;
using FCG.Infrastructure._Common.Auth;
using FCG.Infrastructure._Common.Database;

var builder = WebApplication.CreateBuilder(args);

#region Dependency Injection - DI

builder.Configuration.AddEnvironmentVariables();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddOpenApi()
    .AddDatabases(configuration)
    .AddRepositories()
    .AddInfrastructureServices();

services.ConfigureSettings(configuration)
    .ConfigureAuthentication(configuration)
    .ConfigureAuthorization();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

#region Endpoints

app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapCatalogEndpoints();

#endregion

await app.Services
    .SetupDatabasesAsync(CancellationToken.None);

app.Run();
