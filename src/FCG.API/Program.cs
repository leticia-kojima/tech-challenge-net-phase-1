using FCG.API.Endpoints;
using FCG.API.Middlewares;
using FCG.Infrastructure;
using FCG.Infrastructure._Common.Auth;
using FCG.Infrastructure._Common.Database;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Dependency Injection - DI

var services = builder.Services;
// Adiciona servi�os do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

services.AddOpenApi()
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
    app.UseSwaggerUI();
    app.MapOpenApi();
};

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<UnauthorizedMiddleware>();
app.UseMiddleware<BadRequestMiddleware>();

app.UseHttpsRedirection();
app.UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization();

#region Endpoints

app.MapAuthEndpoints();
app.MapUserEndpoints();

#endregion

await app.Services
    .SetupDatabasesAsync(CancellationToken.None);

app.Run();
