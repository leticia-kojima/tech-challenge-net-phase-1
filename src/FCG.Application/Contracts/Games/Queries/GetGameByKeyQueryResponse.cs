namespace FCG.Application.Contracts.Games.Queries;
public class GetGameByKeyQueryResponse
{
    public required Guid Key { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required Guid CatalogKey { get; set; }
    public required string CatalogName { get; set; }
}

