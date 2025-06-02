using FCG.Application.Contracts.Games.Commands;
using FCG.Application.Contracts.Games.Queries;
using FCG.Domain._Common.Consts;
using FCG.Domain._Common.Exceptions;
using System.Security.Claims;

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

        gamesGroup.MapGet("/{key:guid}", GetGameAsync);        gamesGroup.MapPost("/", CreateGameAsync);

        gamesGroup.MapPost("/{gameKey:guid}/evaluations", CreateGameEvaluationAsync)
            .RequireAuthorization();

        gamesGroup.MapGet("/{gameKey:guid}/evaluations", GetGameEvaluationsAsync);

        gamesGroup.MapGet("/{gameKey:guid}/download", CreateGameDownloadAsync)
            .RequireAuthorization();

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
        );    private static async Task<CreateGameEvaluationCommandResponse> CreateGameEvaluationAsync(
        [FromRoute] Guid catalogKey,
        [FromRoute] Guid gameKey,
        [FromBody] CreateGameEvaluationCommandRequest request,
        [FromServices] IMediator mediator,
        HttpContext httpContext,
        CancellationToken cancellationToken
    )
    {
        // Extract user key from claims
        var userKeyClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userKeyClaim) || !Guid.TryParse(userKeyClaim, out var userKey))
        {
            throw new FCGValidationException("User", "User not authenticated or invalid user identifier.");
        }

        request.CatalogKey = catalogKey;
        request.GameKey = gameKey;
        request.UserKey = userKey;
        return await mediator.Send(request, cancellationToken);
    }

    private static async Task<IReadOnlyCollection<GetGameEvaluationsQueryResponse>> GetGameEvaluationsAsync(
        [FromRoute] Guid catalogKey,
        [FromRoute] Guid gameKey,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(
            new GetGameEvaluationsQueryRequest
            {
                CatalogKey = catalogKey,
                GameKey = gameKey
            },
            cancellationToken
        );    private static async Task<CreateGameDownloadCommandResponse> CreateGameDownloadAsync(
        [FromRoute] Guid catalogKey,
        [FromRoute] Guid gameKey,
        [FromServices] IMediator mediator,
        HttpContext httpContext,
        CancellationToken cancellationToken
    )
    {
        // Extract user key from claims
        var userKeyClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userKeyClaim) || !Guid.TryParse(userKeyClaim, out var userKey))
        {
            throw new FCGValidationException("User", "User not authenticated or invalid user identifier.");
        }

        return await mediator.Send(
            new CreateGameDownloadCommandRequest
            {
                CatalogKey = catalogKey,
                GameKey = gameKey,
                UserKey = userKey
            },
            cancellationToken
        );
    }
}
