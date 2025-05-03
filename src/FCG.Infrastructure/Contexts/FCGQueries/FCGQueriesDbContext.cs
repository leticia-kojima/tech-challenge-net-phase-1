using FCG.Domain.Users;
using MongoFramework;

namespace FCG.Infrastructure.Contexts.FCGQueries;
public class FCGQueriesDbContext : MongoDbContext
{
    public FCGQueriesDbContext(IMongoDbConnection connection) : base(connection) { }

    public MongoDbSet<User> Users { get; set; }
}
