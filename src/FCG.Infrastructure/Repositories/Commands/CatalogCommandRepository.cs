using FCG.Domain.Catalogs;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories.Commands;

public class CatalogCommandRepository : FCGCommandRepositoryBase<Catalog>, ICatalogCommandRepository
{
    public CatalogCommandRepository(FCGCommandsDbContext context) : base(context)
    {
        
    }

    public Task<bool> ExistByNameAsync(string name, Guid? ignoreKey = null, CancellationToken? cancellationToken = null)
        => _dbSet.AnyAsync(u => u.Name == name && u.Key != ignoreKey);
}
