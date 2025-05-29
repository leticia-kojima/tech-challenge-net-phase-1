using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;
using MediatR;

namespace FCG.Application.Events.Games;
public class GameUpdatedEventHandler : IEventHandler<GameUpdatedEvent>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public GameUpdatedEventHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository;
    }

    public async Task Handle(GameUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var game = notification.Game;
        var catalog = await _catalogQueryRepository.GetByIdAsync(game.CatalogKey, cancellationToken);

        if (catalog is null) throw new FCGNotFoundException(game.CatalogKey, nameof(Catalog), $"Catalog with key '{game.CatalogKey}' was not found.");

        catalog.SetGame(game);

        await _catalogQueryRepository.UpdateAsync(catalog, cancellationToken);
    }
}
