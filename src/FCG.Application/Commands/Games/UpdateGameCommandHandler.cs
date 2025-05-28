using FCG.Application.Contracts.Catalogs.Events;
using FCG.Application.Contracts.Games.Commands;
using FCG.Domain.Games;
using FCG.Domain.Users;

namespace FCG.Application.Commands.Games;
public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommandRequest, UpdateGameCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly IGameCommandRepository _gameCommandRepository;

    public UpdateGameCommandHandler(
        IMediator mediator,
        IGameCommandRepository gameCommandRepository
    )
    {
        _mediator = mediator;
        _gameCommandRepository = gameCommandRepository;
    }

    public async Task<UpdateGameCommandResponse> Handle(UpdateGameCommandRequest request, CancellationToken cancellationToken)
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

        var game = await _gameCommandRepository.GetByIdAsync(request.Key, cancellationToken);
        
        if (game is null) throw new FCGNotFoundException(request.Key, nameof(Game), $"Game with key '{request.Key}' was not found.");

        if (game.CatalogKey != request.CatalogKey)
            throw new FCGValidationException(
                nameof(request.CatalogKey),
                $"Game with key '{request.Key}' does not belong to catalog with key '{request.CatalogKey}'."
            );

        var existGameWithSameTitle = await _gameCommandRepository.ExistByTitleAsync(
            request.Title, 
            ignoreKey: request.Key, 
            cancellationToken: cancellationToken
        );
        
        if (existGameWithSameTitle) 
            throw new FCGDuplicateException(nameof(Game), $"The title '{request.Title}' is already in use.");

        game.SetData(
            request.Title,
            request.Description
        );
        
        await _gameCommandRepository.UpdateAsync(game, cancellationToken);

        await _mediator.Publish(new GameUpdatedEvent(game), cancellationToken);

        return new UpdateGameCommandResponse { Key = game.Key };
    }
}
