using FCG.Application.Commands.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoFramework;
using System.Reflection;

namespace FCG.Infrastructure;
public static class FCGInfrastructureModule
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>();
        });

        return services;
    }

    public static IServiceCollection AddDatabases(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<FCGCommandsDbContext>(options => options
            .UseMySql(
                configuration.GetConnectionString("FCGCommands"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("FCGCommands"))
            )
        );
        services.AddScoped<IMongoDbConnection>(sp =>
            MongoDbConnection.FromConnectionString(configuration.GetConnectionString("FCGQueries"))
        );

        services.AddScoped<FCGQueriesDbContext>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
