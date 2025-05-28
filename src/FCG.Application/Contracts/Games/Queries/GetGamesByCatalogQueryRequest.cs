namespace FCG.Application.Contracts.Games.Queries;
public class GetGamesByCatalogQueryRequest : IRequest<IReadOnlyCollection<GetGamesByCatalogQueryResponse>>
{
    public required Guid CatalogKey { get; set; }
}

