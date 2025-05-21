namespace FCG.Application.Contracts.Games.Queries;
public class GetGamesByCatalogQueryRequest : IRequest<IEnumerable<GetGamesByCatalogQueryResponse>>
{
    public required Guid CatalogKey { get; set; }
}

