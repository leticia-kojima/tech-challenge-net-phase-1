using FCG.Domain.Catalogs;

namespace FCG.Application.Contracts.Catalogs.Events;

public class CatalogCreatedEvent : IEvent
{
    public CatalogCreatedEvent(Catalog catalog)
    {
        Catalog = catalog;
    }

    public Catalog Catalog { get; }
}
