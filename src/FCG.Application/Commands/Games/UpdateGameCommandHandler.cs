using FCG.Application.Contracts.Games.Commands;
using FCG.Domain.Games;
using FCG.Domain._Common.Exceptions;

namespace FCG.Application.Commands.Games;
public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommandRequest, UpdateGameCommandResponse>
{
    private readonly IGameCommandRepository _gameCommandRepository;

    public UpdateGameCommandHandler(IGameCommandRepository gameCommandRepository)
    {
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

        var game = await _gameCommandRepository.GetByIdAsync(request.Key, cancellationToken);
        if (game is null)
            throw new FCGNotFoundException(request.Key, nameof(Game), $"Game with id {request.Key} not found");

        var existGameWithSameTitle = await _gameCommandRepository.ExistByTitleAsync(
            request.Title, 
            ignoreKey: request.Key, 
            cancellationToken: cancellationToken
        );
        
        if (existGameWithSameTitle) 
            throw new FCGDuplicateException(nameof(Game), $"The title '{request.Title}' is already in use.");

        // Atualizar os campos da entidade existente
        var catalogKey = request.CatalogKey == Guid.Empty ? game.CatalogKey : request.CatalogKey;
        
        // Obter a entidade atualizada (sem criar uma nova instância)
        var updatedGame = new Game(
            game.Key,
            request.Title,
            request.Description,
            catalogKey
        );
        
        // Desanexar a entidade atual antes de anexar a nova
        _gameCommandRepository.Detach(game);
        
        // Agora podemos atualizar com a nova entidade
        await _gameCommandRepository.UpdateAsync(updatedGame, cancellationToken);

        return new UpdateGameCommandResponse { Key = updatedGame.Key };
    }
}
