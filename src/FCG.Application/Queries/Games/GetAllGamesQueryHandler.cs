using FCG.Application.Contracts.Games.Queries;
using FCG.Domain.Games;
using MediatR;

namespace FCG.Application.Queries.Games;
public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQueryRequest, IEnumerable<GetAllGamesQueryResponse>>
{
    private readonly IGameQueryRepository _gameQueryRepository;

    public GetAllGamesQueryHandler(IGameQueryRepository gameQueryRepository)
    {
        _gameQueryRepository = gameQueryRepository;
    }

    public async Task<IEnumerable<GetAllGamesQueryResponse>> Handle(GetAllGamesQueryRequest request, CancellationToken cancellationToken)
    {
        var games = await _gameQueryRepository.GetAllAsync(cancellationToken);
        
        return games.Select(game => new GetAllGamesQueryResponse
        {
            Key = game.Key,
            Title = game.Title,
            Description = game.Description,
            CatalogKey = game.CatalogKey,
            CatalogName = game.Catalog?.Name ?? "Unknown",
            CreatedAt = game.CreatedAt,
            UpdatedAt = game.UpdatedAt,
            DeletedAt = game.DeletedAt
        }).ToList();
    }
}
