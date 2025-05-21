using FCG.Application.Contracts.Games.Commands;
using FCG.Domain.Games;
using FCG.Domain._Common.Exceptions;

namespace FCG.Application.Commands.Games;
public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommandRequest>
{
    private readonly IGameCommandRepository _gameCommandRepository;

    public DeleteGameCommandHandler(IGameCommandRepository gameCommandRepository)
    {
        _gameCommandRepository = gameCommandRepository;
    }

    public async Task Handle(DeleteGameCommandRequest request, CancellationToken cancellationToken)
    {
        var game = await _gameCommandRepository.GetByIdAsync(request.Key, cancellationToken);
        if (game is null)
            throw new FCGNotFoundException(request.Key, nameof(Game), $"Game with id {request.Key} not found");

        await _gameCommandRepository.DeleteAsync(game, cancellationToken);
    }
}
