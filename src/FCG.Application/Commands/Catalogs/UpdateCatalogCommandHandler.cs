using FCG.Application.Contracts.Catalogs.Commands;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;

namespace FCG.Application.Commands.Catalogs;

public class UpdateCatalogCommandHandler : ICommandHandler<UpdateCatalogCommandRequest, UpdateCatalogCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly ICatalogCommandRepository _catalogCommandRepository;

    public UpdateCatalogCommandHandler(
        IMediator mediator,
        ICatalogCommandRepository catalogCommandRepository)
    {
        _mediator = mediator;
        _catalogCommandRepository = catalogCommandRepository;
    }

    public async Task<UpdateCatalogCommandResponse> Handle(UpdateCatalogCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new FCGValidationException(
                nameof(request.Name),
                $"{nameof(request.Name)} is required."
            );

        if (string.IsNullOrWhiteSpace(request.Description))
            throw new FCGValidationException(
                nameof(request.Description),
                $"{nameof(request.Description)} is required."
            );

        var catalog = await _catalogCommandRepository.GetByIdAsync(request.Key, cancellationToken);

        if (catalog is null) throw new FCGNotFoundException(request.Key, nameof(Catalog), $"Catalog with key '{request.Key}' was not found.");

        var existCatalogWithSameName = await _catalogCommandRepository.ExistByNameAsync(
            request.Name,
            request.Key,
            cancellationToken
        );

        if (existCatalogWithSameName) throw new FCGDuplicateException(nameof(Catalog), $"The name '{request.Name}' is already in use.");

        catalog.SetData(request.Name, request.Description);

        await _catalogCommandRepository.UpdateAsync(catalog, cancellationToken);

        await _mediator.Publish(new CatalogUpdatedEvent(catalog), cancellationToken);
         
        return new UpdateCatalogCommandResponse(catalog);
    }
}
