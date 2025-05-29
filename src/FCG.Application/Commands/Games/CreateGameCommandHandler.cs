using FCG.Application.Contracts.Catalogs.Events;
using FCG.Application.Contracts.Games.Commands;
using FCG.Domain.Games;

namespace FCG.Application.Commands.Games;
public class CreateGameCommandHandler : IRequestHandler<CreateGameCommandRequest, CreateGameCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly IGameCommandRepository _gameCommandRepository;

    public CreateGameCommandHandler(
        IMediator mediator,
        IGameCommandRepository gameCommandRepository
    )
    {
        _mediator = mediator;
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

        if (request.Title.Length > 100)
            throw new FCGValidationException(nameof(request.Title), "Title cannot exceed 100 characters.");

        if (request.Description.Length > 500)
            throw new FCGValidationException(nameof(request.Description), "Description cannot exceed 500 characters.");

        var existGameWithSameTitle = await _gameCommandRepository.ExistByTitleAsync(request.Title, cancellationToken: cancellationToken);
        
        if (existGameWithSameTitle) throw new FCGDuplicateException(nameof(Game), $"The title '{request.Title}' is already in use.");

        var game = new Game(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.CatalogKey
        );
        await _gameCommandRepository.AddAsync(game, cancellationToken);

        await _mediator.Publish(new GameCreatedEvent(game), cancellationToken);

        return new CreateGameCommandResponse(game);
    }
}
