using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;
using MediatR;

namespace FCG.Application.Events.Games;
public class GameCreatedEventHandler : IEventHandler<GameCreatedEvent>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public GameCreatedEventHandler(ICatalogQueryRepository catalogQueryRepository)
    {
        _catalogQueryRepository = catalogQueryRepository;
    }

    public async Task Handle(GameCreatedEvent notification, CancellationToken cancellationToken)
    {
        var game = notification.Game;
        var catalog = await _catalogQueryRepository.GetByIdAsync(game.CatalogKey, cancellationToken);

        if (catalog is null) throw new FCGNotFoundException(game.CatalogKey, nameof(Catalog), $"Catalog with key '{game.CatalogKey}' was not found.");

        catalog.AddGame(game);

        await _catalogQueryRepository.UpdateAsync(catalog, cancellationToken);
    }
}
