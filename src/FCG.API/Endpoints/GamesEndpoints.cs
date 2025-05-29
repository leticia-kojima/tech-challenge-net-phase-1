using FCG.Application.Contracts.Games.Commands;
using FCG.Application.Contracts.Games.Queries;
using FCG.Domain._Common.Consts;

namespace FCG.API.Endpoints;

public static class GamesEndpoints
{
    public static void MapGamesEndpoints(this WebApplication app)
    {
        const string gamesRoute = "catalogs/{catalogKey:guid}/games";
        var gamesGroup = app.MapGroup(gamesRoute);
        var gamesGroupOnlyForAdmin = app.MapGroup(gamesRoute)
            .RequireAuthorization(Policies.OnlyAdmin);

        gamesGroup.MapGet("/", GetGamesAsync);

        gamesGroup.MapGet("/{key:guid}", GetGameAsync);

        gamesGroup.MapPost("/", CreateGameAsync);

        // TODO: POST {{FCG.API_HostAddress}}/catalogs/{{$guid}}/games/{{$guid}}/evaluations

        // TODO: GET {{FCG.API_HostAddress}}/catalogs/{{$guid}}/games/{{$guid}}/evaluations

        // TODO: GET {{FCG.API_HostAddress}}/catalogs/{{$guid}}/games/{{$guid}}/download

        gamesGroup.MapPut("/{key:guid}", UpdateGameAsync);

        gamesGroup.MapDelete("/{key:guid}", DeleteGameAsync);
    }

    private static async Task<IReadOnlyCollection<GetGamesByCatalogQueryResponse>> GetGamesAsync(
        [FromRoute] Guid catalogKey,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(
            new GetGamesByCatalogQueryRequest { CatalogKey = catalogKey },
            cancellationToken
        );

    private static async Task<GetGameByKeyQueryResponse> GetGameAsync(
        [FromRoute] Guid catalogKey,
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(
            new GetGameByKeyQueryRequest {
                CatalogKey = catalogKey,
                Key = key
            },
            cancellationToken
        );

    private static async Task<CreateGameCommandResponse> CreateGameAsync(
        [FromRoute] Guid catalogKey,
        [FromBody] CreateGameCommandRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        request.CatalogKey = catalogKey;
        return await mediator.Send(request, cancellationToken);
    }

    private static async Task<UpdateGameCommandResponse> UpdateGameAsync(
        [FromRoute] Guid catalogKey,
        [FromRoute] Guid key,
        [FromBody] UpdateGameCommandRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        request.Key = key;
        request.CatalogKey = catalogKey;
        return await mediator.Send(request, cancellationToken);
    }
    
    private static async Task DeleteGameAsync(
        [FromRoute] Guid catalogKey,
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(
            new DeleteGameCommandRequest { 
                CatalogKey = catalogKey,
                Key = key
            },
            cancellationToken
        );
}
