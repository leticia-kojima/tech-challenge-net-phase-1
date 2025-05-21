using FCG.Domain.Games;
using MongoFramework.Linq;

namespace FCG.Infrastructure.Repositories.Queries;
public class GameQueryRepository : FCGQueryRepositoryBase<Game>, IGameQueryRepository
{
    public GameQueryRepository(FCGQueriesDbContext context) : base(context)
    {
    }

    public async Task<Game?> GetByKeyAsync(Guid key, CancellationToken? cancellationToken = null)
    {
        return await Query
            .Where(g => g.Key == key)
            .FirstOrDefaultAsync(cancellationToken ?? CancellationToken.None);
    }
    
    public async Task<IEnumerable<Game>> GetAllAsync(CancellationToken? cancellationToken = null)
    {
        return await Query
            .ToArrayAsync(cancellationToken ?? CancellationToken.None);
    }
    
    public async Task<IEnumerable<Game>> GetByCatalogKeyAsync(Guid catalogKey, CancellationToken? cancellationToken = null)
    {
        return await Query
            .Where(g => g.CatalogKey == catalogKey)
            .ToArrayAsync(cancellationToken ?? CancellationToken.None);
    }
}
