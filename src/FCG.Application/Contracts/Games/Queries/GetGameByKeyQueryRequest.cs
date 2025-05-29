namespace FCG.Application.Contracts.Games.Queries;
public class GetGameByKeyQueryRequest : IRequest<GetGameByKeyQueryResponse>
{
    public required Guid Key { get; set; }
    public Guid CatalogKey { get; set; }
}

