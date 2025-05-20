using FCG.Application._Common;
using FCG.Application.Contracts.Catalogs.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Queries.Catalogs;

public class ListCatalogQueryHandler : IQueryHandler<ListCatalogsQueryRequest, IReadOnlyCollection<CatalogQueryResponse>>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public ListCatalogQueryHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository;
    }

    public async Task<IReadOnlyCollection<CatalogQueryResponse>> Handle(ListCatalogsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var catalogs = await _catalogQueryRepository.SearchAsync(request.Search, cancellationToken);

        return catalogs.Select(u => new CatalogQueryResponse(u)).ToArray();
    }
}
