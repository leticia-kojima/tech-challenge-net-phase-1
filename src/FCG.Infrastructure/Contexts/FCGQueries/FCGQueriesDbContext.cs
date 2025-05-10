using FCG.Domain._Common;
using FCG.Domain.Catalogs;
using FCG.Domain.Games;
using FCG.Domain.Users;
using FCG.Infrastructure.Mappings.Queries.Collections;
using FCG.Infrastructure.Mappings.Queries.Objects;
using FCG.Infrastructure.Seeders;
using MongoFramework;
using MongoFramework.Linq;

namespace FCG.Infrastructure.Contexts.FCGQueries;
public class FCGQueriesDbContext : MongoDbContext
{
    public FCGQueriesDbContext(IMongoDbConnection connection) : base(connection) { }

    public MongoDbSet<User> Users { get; set; }
    public MongoDbSet<Catalog> Catalogs { get; set; }

    protected override void OnConfigureMapping(MappingBuilder mappingBuilder)
    {
        mappingBuilder
            .Entity<EntityBase>()
            .HasKey(e => e.Key);

        new UserCollectionMapping(mappingBuilder.Entity<User>()).Configure();
        new CatalogCollectionMapping(mappingBuilder.Entity<Catalog>()).Configure();
        
        new GameObjectMapping(mappingBuilder.Entity<Game>()).Configure();
        new GameDownloadObjectMapping(mappingBuilder.Entity<GameDownload>()).Configure();
        new GameEvaluationObjectMapping(mappingBuilder.Entity<GameEvaluation>()).Configure();

        base.OnConfigureMapping(mappingBuilder);
    }

    public async Task SeedAllDataAsync(CancellationToken cancellationToken)
    {
        if (!await Users.AnyAsync()) Users.AddRange(new UserSeeder().GetData());
        
        if (!await Catalogs.AnyAsync()) Catalogs.AddRange(new CatalogSeeder().GetData());

        await SaveChangesAsync(cancellationToken);
    }
}
