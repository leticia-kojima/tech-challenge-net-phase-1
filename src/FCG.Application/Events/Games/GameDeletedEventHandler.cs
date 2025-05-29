using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;

namespace FCG.Application.Events.Games;
public class GameDeletedEventHandler : IEventHandler<GameDeletedEvent>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public GameDeletedEventHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository;
    }

    public async Task Handle(GameDeletedEvent notification, CancellationToken cancellationToken)
    {
        var game = notification.Game;
        var catalog = await _catalogQueryRepository.GetByIdAsync(game.CatalogKey, cancellationToken);

        if (catalog is null) throw new FCGNotFoundException(game.CatalogKey, nameof(Catalog), $"Catalog with key '{game.CatalogKey}' was not found.");

        catalog.RemoveGame(game);

        await _catalogQueryRepository.UpdateAsync(catalog, cancellationToken);
    }
}
