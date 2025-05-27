using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;

namespace FCG.Application.Events.Catalogs;

public class CatalogUpdatedEventHandler : IEventHandler<CatalogUpdatedEvent>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public CatalogUpdatedEventHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository;
    }

    public async Task Handle(CatalogUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _catalogQueryRepository.UpdateAsync(notification.Catalog, cancellationToken);
    }
}
