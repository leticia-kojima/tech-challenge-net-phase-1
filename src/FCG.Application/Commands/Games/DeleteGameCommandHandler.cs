using FCG.Application.Contracts.Catalogs.Events;
using FCG.Application.Contracts.Games.Commands;
using FCG.Domain.Games;
using FCG.Domain.Users;

namespace FCG.Application.Commands.Games;
public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommandRequest>
{
    private readonly IMediator _mediator;
    private readonly IGameCommandRepository _gameCommandRepository;

    public DeleteGameCommandHandler(
        IMediator mediator,
        IGameCommandRepository gameCommandRepository
    )
    {
        _mediator = mediator;
        _gameCommandRepository = gameCommandRepository;
    }

    public async Task Handle(DeleteGameCommandRequest request, CancellationToken cancellationToken)
    {
        var game = await _gameCommandRepository.GetByIdAsync(request.Key, cancellationToken);

        if (game is null) throw new FCGNotFoundException(request.Key, nameof(Game), $"Game with key '{request.Key}' was not found.");

        if (game.CatalogKey != request.CatalogKey)
            throw new FCGValidationException(
                nameof(request.CatalogKey),
                $"Game with key '{request.Key}' does not belong to catalog with key '{request.CatalogKey}'."
            );

        await _gameCommandRepository.DeleteAsync(game, cancellationToken);

        await _mediator.Publish(new GameDeletedEvent(game), cancellationToken);
    }
}
