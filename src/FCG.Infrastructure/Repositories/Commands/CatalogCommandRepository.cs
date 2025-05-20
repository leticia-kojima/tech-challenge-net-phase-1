using FCG.Infrastructure._Common.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Infrastructure.Repositories.Commands;

public class CatalogCommandRepository : FCGCommandRepositoryBase<Catalog>, ICatalogCommandRepository
{
    public CatalogCommandRepository()
    {
        
    }

    public Task<bool> ExistByNameAsync(string name, Guid? ignoreKey = null, CancellationToken? cancellationToken = null)
        => _dbSet.AnyAsync(u => u.Name == name && u.Key != ignoreKey);
}
