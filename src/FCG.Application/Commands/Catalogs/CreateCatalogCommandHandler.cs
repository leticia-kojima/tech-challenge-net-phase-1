using FCG.Application.Contracts.Catalogs.Commands;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;

namespace FCG.Application.Commands.Catalogs;

public class CreateCatalogCommandHandler : ICommandHandler<CreateCatalogCommandRequest, CreateCatalogCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly ICatalogCommandRepository _catalogCommandRepository;

    public CreateCatalogCommandHandler(
        IMediator mediator,
        ICatalogCommandRepository catalogCommandRepository)
    {
        _mediator = mediator;
        _catalogCommandRepository = catalogCommandRepository;
    }

    public async Task<CreateCatalogCommandResponse> Handle(CreateCatalogCommandRequest request,
        CancellationToken cancellationToken)
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

        var existCatalogWithSameName = await _catalogCommandRepository.ExistByNameAsync(request.Name, cancellationToken: cancellationToken);

        if (existCatalogWithSameName) throw new FCGDuplicateException(nameof(Catalog), $"The name '{request.Name}' is already in use.");

        var catalog = new Catalog(
            Guid.NewGuid(),
            request.Name,
            request.Description
        );
        await _catalogCommandRepository.AddAsync(catalog, cancellationToken);

        await _mediator.Publish(new CatalogCreatedEvent(catalog), cancellationToken);

        return new CreateCatalogCommandResponse(catalog);
    }
}
