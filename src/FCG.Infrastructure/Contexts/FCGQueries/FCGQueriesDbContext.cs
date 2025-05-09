using FCG.Domain._Common;
using FCG.Domain.Catalogs;
using FCG.Domain.Users;
using FCG.Infrastructure.Mappings.Queries;
using MongoFramework;

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

        new UserMapping(mappingBuilder.Entity<User>()).Configure();

        base.OnConfigureMapping(mappingBuilder);
    }
}
