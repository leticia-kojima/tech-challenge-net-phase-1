using FCG.Infrastructure.Mappings.Queries.Serializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace FCG.Infrastructure._Common.Database;
public static class ContextSetup
{
    public static async Task SetupDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken)
    {
        using var scope = services.CreateScope();

        await SetupMySqlDatabaseAsync(
            scope.ServiceProvider.GetRequiredService<FCGCommandsDbContext>(),
            cancellationToken
        );

        await SetupMongoDatabaseAsync(
            scope.ServiceProvider.GetRequiredService<FCGQueriesDbContext>(),
            cancellationToken
        );
    }

    private static Task SetupMySqlDatabaseAsync(
        FCGCommandsDbContext context,
        CancellationToken cancellationToken
    ) => context.Database.MigrateAsync(cancellationToken);

    private static async Task SetupMongoDatabaseAsync(
        FCGQueriesDbContext context,
        CancellationToken cancellationToken
    )
    {
        BsonSerializer.RegisterSerializer(new EmailSerializer());
        BsonSerializer.RegisterSerializer(new PasswordSerializer());

        await context.SeedAllDataAsync(cancellationToken);
    }
}
