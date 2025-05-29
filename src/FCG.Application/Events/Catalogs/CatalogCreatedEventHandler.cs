using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;

namespace FCG.Application.Events.Catalogs;

public class CatalogCreatedEventHandler : IEventHandler<CatalogCreatedEvent>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public CatalogCreatedEventHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository;
    }

    public async Task Handle(CatalogCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _catalogQueryRepository.AddAsync(notification.Catalog, cancellationToken);
    }
}
