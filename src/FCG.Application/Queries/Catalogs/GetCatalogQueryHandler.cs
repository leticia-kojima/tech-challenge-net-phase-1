using FCG.Application.Contracts.Catalogs.Queries;
using FCG.Domain.Catalogs;

namespace FCG.Application.Queries.Catalogs;

public class GetCatalogQueryHandler : IQueryHandler<GetCatalogQueryRequest, CatalogQueryResponse>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public GetCatalogQueryHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository; 
    }
    
    public async Task<CatalogQueryResponse> Handle(GetCatalogQueryRequest request, CancellationToken cancellationToken)
    {
        var catalog = await _catalogQueryRepository.GetByIdAsync(request.Key, cancellationToken);

        if (catalog is null) throw new FCGNotFoundException(request.Key, nameof(Catalog), $"Catalog with key '{request.Key}' was not found.");

        return new CatalogQueryResponse(catalog);
    }
}
