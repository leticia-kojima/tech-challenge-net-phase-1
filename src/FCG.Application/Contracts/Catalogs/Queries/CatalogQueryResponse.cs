using FCG.Application.Contracts.Catalogs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Contracts.Catalogs.Queries;

public class CatalogQueryResponse : CatalogCommandModelBase
{
    public CatalogQueryResponse(Catalog catalog) : base(catalog)
    {
        Key = catalog.Key;
    }

    public Guid Key { get; set; }
}
