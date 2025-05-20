using FCG.Infrastructure.Contexts.FCGQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Repositories.Queries;

public class CatalogQueryRepository : FCGQueryRepositoryBase<Catalog>, ICatalogQueryRepository
{
    public CatalogQueryRepository(FCGQueriesDbContext context) : base(context)
    {        
    }

    public async Task<IReadOnlyCollection<Catalog>> SearchAsync(string? search, CancellationToken cancellationToken)
    {
        var query = Query;

        if (!string.IsNullOrEmpty(search))
        {
            var searchRegex = new Regex(Regex.Escape(search), RegexOptions.IgnoreCase);
            query = query.Where(u => searchRegex.IsMatch(u.Name));
        }

        return await query.ToArrayAsync(cancellationToken);
    }

}
