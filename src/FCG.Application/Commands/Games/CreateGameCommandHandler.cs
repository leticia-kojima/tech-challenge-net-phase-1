using FCG.Application.Contracts.Games.Commands;
using FCG.Domain.Games;
using MediatR;

namespace FCG.Application.Commands.Games;
public class CreateGameCommandHandler : IRequestHandler<CreateGameCommandRequest, CreateGameCommandResponse>
{
    private readonly IGameCommandRepository _gameCommandRepository;

    public CreateGameCommandHandler(IGameCommandRepository gameCommandRepository)
    {
        _gameCommandRepository = gameCommandRepository;
    }

    public async Task<CreateGameCommandResponse> Handle(CreateGameCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new FCGValidationException(
                nameof(request.Title),
                $"{nameof(request.Title)} is required."
            );

        if (string.IsNullOrWhiteSpace(request.Description))
            throw new FCGValidationException(
                nameof(request.Description),
                $"{nameof(request.Description)} is required."
            );

        var existGameWithSameTitle = await _gameCommandRepository.ExistByTitleAsync(request.Title, cancellationToken: cancellationToken);
        if (existGameWithSameTitle) 
            throw new FCGDuplicateException(nameof(Game), $"The title '{request.Title}' is already in use.");

        var game = new Game(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.CatalogKey
        );

        await _gameCommandRepository.AddAsync(game, cancellationToken);

        return new CreateGameCommandResponse { Key = game.Key };
    }
}
