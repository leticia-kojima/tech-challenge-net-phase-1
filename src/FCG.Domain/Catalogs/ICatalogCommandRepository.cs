using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Domain.Catalogs;

public interface ICatalogCommandRepository : ICatalogRepository
{
    Task<bool> ExistByEmailAsync(
        string name,
        Guid? ignoreKey = null,
        CancellationToken? cancellationToken = null
    );
}
