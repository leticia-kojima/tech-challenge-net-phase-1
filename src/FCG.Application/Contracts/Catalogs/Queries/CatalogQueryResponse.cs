using FCG.Application.Contracts.Catalogs.Abstract;
using FCG.Domain.Catalogs;

namespace FCG.Application.Contracts.Catalogs.Queries;

public class CatalogQueryResponse : CatalogCommandModelBase
{
    public CatalogQueryResponse(Catalog catalog) : base(catalog)
    {
        Key = catalog.Key;
    }

    public Guid Key { get; set; }
}
