using FCG.Application.Contracts.Games.Queries;
using FCG.Domain.Catalogs;
using FCG.Domain.Games;

public class GetGamesByCatalogQueryHandler : IRequestHandler<GetGamesByCatalogQueryRequest, IReadOnlyCollection<GetGamesByCatalogQueryResponse>>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public GetGamesByCatalogQueryHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository;
    }

    public async Task<IReadOnlyCollection<GetGamesByCatalogQueryResponse>> Handle(GetGamesByCatalogQueryRequest request, CancellationToken cancellationToken)
    {
        var catalog = await _catalogQueryRepository.GetByIdAsync(request.CatalogKey, cancellationToken);

        if (catalog is null) throw new FCGNotFoundException(request.CatalogKey, nameof(Catalog), $"Catalog with key '{request.CatalogKey}' was not found.");

        return catalog.Games.Select(g => new GetGamesByCatalogQueryResponse(g)).ToArray();
    }
}