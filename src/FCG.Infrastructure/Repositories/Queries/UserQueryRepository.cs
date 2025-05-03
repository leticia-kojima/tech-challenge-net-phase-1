using FCG.Domain.Users;
using FCG.Infrastructure._Common.Persistence;
using FCG.Infrastructure.Contexts.FCGQueries;
using MongoFramework.Linq;

namespace FCG.Infrastructure.Repositories.Queries;
public class UserQueryRepository : FCGQueryRepositoryBase<User>, IUserQueryRepository
{
    public UserQueryRepository(FCGQueriesDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> SearchAsync(string search, CancellationToken cancellationToken)
    {
        return await _dbSet.SearchText(search).ToArrayAsync(cancellationToken);
    }
}