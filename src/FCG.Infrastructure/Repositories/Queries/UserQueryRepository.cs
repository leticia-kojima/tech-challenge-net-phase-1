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

    public async Task<IEnumerable<User>> SearchAsync(string? search, CancellationToken cancellationToken)
    {
        var query = _dbSet
            .Where(u => u.DeletedAt == null);

        if (!string.IsNullOrEmpty(search))
        {
            var loweredSearch = search.ToLower();

            query = query.Where(u => u.FullName.ToLower().Contains(loweredSearch));
        }

        return await query.ToArrayAsync(cancellationToken);
    }
}