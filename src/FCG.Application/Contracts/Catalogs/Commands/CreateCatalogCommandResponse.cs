using FCG.Application.Contracts.Catalogs.Abstract;
using FCG.Domain.Catalogs;

namespace FCG.Application.Contracts.Catalogs.Commands;

public class CreateCatalogCommandResponse : CatalogCommandModelBase
{
    public CreateCatalogCommandResponse(Catalog catalog) : base(catalog)
    {
        Key = catalog.Key;
    }

    public Guid Key { get; set; }
}
