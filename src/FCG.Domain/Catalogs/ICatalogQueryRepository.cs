using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Domain.Catalogs;

public interface ICatalogQueryRepository : ICatalogRepository
{
    Task<IReadOnlyCollection<Catalog>> SearchAsync(string? search, CancellationToken cancellationToken);
}
