using FCG.Application.Contracts.Games.Queries;
using FCG.Domain.Games;
using MediatR;

namespace FCG.Application.Queries.Games;
public class GetGameByKeyQueryHandler : IRequestHandler<GetGameByKeyQueryRequest, GetGameByKeyQueryResponse>
{
    private readonly IGameQueryRepository _gameQueryRepository;

    public GetGameByKeyQueryHandler(IGameQueryRepository gameQueryRepository)
    {
        _gameQueryRepository = gameQueryRepository;
    }

    public async Task<GetGameByKeyQueryResponse> Handle(GetGameByKeyQueryRequest request, CancellationToken cancellationToken)
    {
        var game = await _gameQueryRepository.GetByKeyAsync(request.Key, cancellationToken);
        
        if (game == null)
            return null;
            
        return new GetGameByKeyQueryResponse
        {
            Key = game.Key,
            Title = game.Title,
            Description = game.Description,
            CatalogKey = game.CatalogKey,
            CatalogName = game.Catalog?.Name ?? "undefined"
        };
    }
}
