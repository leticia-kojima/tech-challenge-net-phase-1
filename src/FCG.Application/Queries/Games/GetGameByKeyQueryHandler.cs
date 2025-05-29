using FCG.Application.Contracts.Games.Queries;
using FCG.Domain.Catalogs;
using FCG.Domain.Games;
using FCG.Domain.Users;

namespace FCG.Application.Queries.Games;
public class GetGameByKeyQueryHandler : IRequestHandler<GetGameByKeyQueryRequest, GetGameByKeyQueryResponse>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public GetGameByKeyQueryHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository;
    }

    public async Task<GetGameByKeyQueryResponse> Handle(GetGameByKeyQueryRequest request, CancellationToken cancellationToken)
    {
        var catalog = await _catalogQueryRepository.GetByIdAsync(request.CatalogKey, cancellationToken);

        if (catalog is null) throw new FCGNotFoundException(request.CatalogKey, nameof(Catalog), $"Catalog with key '{request.CatalogKey}' was not found.");

        return new GetGameByKeyQueryResponse(catalog.GetGame(request.Key), catalog.Name);
    }
}
