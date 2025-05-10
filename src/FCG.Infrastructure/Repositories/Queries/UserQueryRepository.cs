using FCG.Domain.Users;
using MongoFramework.Linq;
using System.Text.RegularExpressions;

namespace FCG.Infrastructure.Repositories.Queries;
public class UserQueryRepository : FCGQueryRepositoryBase<User>, IUserQueryRepository
{
    public UserQueryRepository(FCGQueriesDbContext context) : base(context)
    {

    }

    public async Task<IReadOnlyCollection<User>> SearchAsync(string? search, CancellationToken cancellationToken)
    {
        var query = Query;

        if (!string.IsNullOrEmpty(search))
        {
            var searchRegex = new Regex(Regex.Escape(search), RegexOptions.IgnoreCase);

            query = query.Where(u => searchRegex.IsMatch(u.FullName));
        }

        return await query.ToArrayAsync(cancellationToken);
    }
}