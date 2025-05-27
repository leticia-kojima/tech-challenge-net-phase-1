using FCG.Application._Common.Handlers;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
