using FCG.Application._Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Contracts.Catalogs.Queries;

public class GetCatalogQueryRequest : IQuery<CatalogQueryResponse>
{
    public Guid Key { get; set; }
}
