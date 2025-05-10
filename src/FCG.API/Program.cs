using FCG.API.Endpoints;
using FCG.Infrastructure;
using FCG.Infrastructure.Contexts.FCGQueries;

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

app.UseHttpsRedirection();

#region Endpoints

app.MapUserEndpoints();

#endregion

#region Seed

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FCGQueriesDbContext>();
    await dbContext.SeedAllDataAsync(CancellationToken.None);
}

#endregion

app.Run();
