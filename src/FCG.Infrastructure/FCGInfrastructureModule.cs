using FCG.Application.Commands.Users;
using FCG.Domain._Common.Settings;
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
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>());

        return services;
    }

    public static IServiceCollection AddDatabases(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var mySqlConnection = Environment.GetEnvironmentVariable("FCGCommands")
                          ?? configuration.GetConnectionString("FCGCommands");

        var mongoConnection = Environment.GetEnvironmentVariable("FCGQueries")
                              ?? configuration.GetConnectionString("FCGQueries");

        services.AddDbContext<FCGCommandsDbContext>(options => options
            .UseMySql(
                mySqlConnection,
                ServerVersion.AutoDetect(mySqlConnection)
            )
        );
        services.AddScoped<IMongoDbConnection>(sp =>
            MongoDbConnection.FromConnectionString(mongoConnection)
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

    public static IServiceCollection ConfigureSettings(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        return services;
    }
}
