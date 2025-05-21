using FCG.Domain.Catalogs;

namespace FCG.Application.Contracts.Catalogs.Events;

public class CatalogDeletedEvent : IEvent
{
    public CatalogDeletedEvent(Catalog catalog)
    {
        Catalog = catalog;
    }

    public Catalog Catalog { get;}
}
