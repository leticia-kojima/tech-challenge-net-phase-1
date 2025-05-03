using FCG.API.Endpoints;
using FCG.Application.Commands.Users;
using FCG.Infrastructure._Common.Mapping;
using FCG.Infrastructure.Contexts.FCGCommands;
using FCG.Infrastructure.Contexts.FCGQueries;
using FCG.Infrastructure.Repositories.Commands;
using Microsoft.EntityFrameworkCore;
using MongoFramework;

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

services.AddSingleton<IMongoDbConnection>(sp =>
    MongoDbConnection.FromConnectionString(configuration.GetConnectionString("FCGQueries"))
);

services.AddScoped<FCGQueriesDbContext>();

services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>());

services.Scan(scan => scan
    .FromAssemblyOf<UserCommandRepository>()
    .AddClasses(classes => classes
        .Where(c => c.Name.EndsWith("CommandRepository")
            || c.Name.EndsWith("QueryRepository")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
);

#endregion

QueryMappings.RegisterMappings();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

#region Endpoints

app.MapUserEndpoints();

#endregion

app.Run();
