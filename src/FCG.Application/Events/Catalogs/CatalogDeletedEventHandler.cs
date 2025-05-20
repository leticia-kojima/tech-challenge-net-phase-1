using FCG.Application._Common.Handlers;
using FCG.Application.Contracts.Catalogs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Events.Catalogs;

public class CatalogDeletedEventHandler : IEventHandler<CatalogDeletedEvent>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public CatalogDeletedEventHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository;
    }

    public async Task Handle(CatalogDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _catalogQueryRepository.DeleteAsync(notification.Catalog, cancellationToken);
    }
}
