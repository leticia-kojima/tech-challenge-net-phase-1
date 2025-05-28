using FCG.Domain.Games;

namespace FCG.Application.Contracts.Games.Queries;
public class GetGameByKeyQueryResponse
{
    public GetGameByKeyQueryResponse(Game game)
    {
        Key = game.Key;
        Title = game.Title;
        Description = game.Description;
        CatalogKey = game.CatalogKey;
        CatalogName = game.Catalog.Name;
    }

    public Guid Key { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CatalogKey { get; set; }
    public string CatalogName { get; set; }
}

