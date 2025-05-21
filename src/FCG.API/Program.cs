using FCG.API.Endpoints;
using FCG.API.Middlewares;
using FCG.Infrastructure;
using FCG.Infrastructure._Common.Database;

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

// Adiciona o middleware de tratamento global de exceções
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

#region Endpoints

app.MapUserEndpoints();
app.MapGamesEndpoints();

#endregion

await app.Services
    .SetupDatabasesAsync(CancellationToken.None);

app.Run();
