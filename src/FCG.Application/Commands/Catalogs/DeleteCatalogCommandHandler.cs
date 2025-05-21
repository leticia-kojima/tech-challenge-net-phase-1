using FCG.Application.Contracts.Catalogs.Commands;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Commands.Catalogs;

public class DeleteCatalogCommandHandler
{
    private readonly IMediator _mediator;
    private readonly ICatalogCommandRepository _catalogCommandRepository;

    public DeleteCatalogCommandHandler(
        IMediator mediator,
        ICatalogCommandRepository catalogCommandRepository)
    {
        _mediator = mediator;
        _catalogCommandRepository = catalogCommandRepository;
    }

    public async Task Handle(DeleteCatalogCommandRequest request, CancellationToken cancellationToken)
    {
        var catalog = await _catalogCommandRepository.GetByIdAsync(request.Key, cancellationToken);
        if (catalog is null) throw new FCGNotFoundException(request.Key, nameof(Catalog), $"Catalog with key '{request.Key}' was not found.");

        await _catalogCommandRepository.DeleteAsync(catalog, cancellationToken);
        await _mediator.Publish(new CatalogDeletedEvent(catalog), cancellationToken);
    }
}
