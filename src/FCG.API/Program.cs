using FCG.API.Endpoints;
using FCG.Infrastructure;
using FCG.Infrastructure._Common.Database;

var builder = WebApplication.CreateBuilder(args);

#region Dependency Injection - DI

var services = builder.Services;
// Adiciona serviços do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

services.AddOpenApi()
    .AddDatabases(configuration)
    .AddRepositories()
    .AddInfrastructureServices();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
};

app.UseHttpsRedirection();

#region Endpoints

app.MapUserEndpoints();

#endregion

await app.Services
    .SetupDatabasesAsync(CancellationToken.None);

app.Run();
