using FCG.API.Endpoints;
using FCG.Infrastructure.Contexts.FCGCommands;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Dependency Injection - DI

var services = builder.Services;
var configuration = builder.Configuration;

services.AddOpenApi();

services.AddDbContext<FCGCommandsDbContext>(options => options
    .UseMySql(
        configuration.GetConnectionString("FCGCommands"),
        ServerVersion.AutoDetect(configuration.GetConnectionString("FCGCommands"))
    )
);

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

#region Endpoints

app.MapUserEndpoints();

#endregion

app.Run();
