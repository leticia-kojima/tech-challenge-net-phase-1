using FCG.Domain.Catalogs;

namespace FCG.Application.Contracts.Catalogs.Abstract;

public abstract class CatalogCommandModelBase
{
    protected CatalogCommandModelBase()
    {            
    }

    protected CatalogCommandModelBase(Catalog catalog)
    {
        Name = catalog.Name;
        Description = catalog.Description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}
