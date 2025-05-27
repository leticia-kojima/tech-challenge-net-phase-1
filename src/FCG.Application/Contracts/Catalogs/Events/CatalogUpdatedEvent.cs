using FCG.Domain.Catalogs;

namespace FCG.Application.Contracts.Catalogs.Events;

public class CatalogUpdatedEvent : IEvent
{
    public CatalogUpdatedEvent(Catalog catalog)
    {
        Catalog = catalog;
    }

    public Catalog Catalog { get; }
}
