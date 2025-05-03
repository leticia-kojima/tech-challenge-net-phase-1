using FCG.API.Endpoints;
using FCG.Application.Commands.Users;
using FCG.Infrastructure.Contexts.FCGCommands;
using FCG.Infrastructure.Repositories.Commands;
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

services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>());

builder.Services.Scan(scan => scan
    .FromAssemblyOf<UserCommandRepository>()
    .AddClasses(classes => classes
        .Where(c => c.Name.EndsWith("CommandRepository")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
);

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

#region Endpoints

app.MapUserEndpoints();

#endregion

app.Run();
