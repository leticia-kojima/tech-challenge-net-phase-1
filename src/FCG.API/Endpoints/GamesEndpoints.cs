using FCG.Application.Contracts.Games.Commands;
using FCG.Application.Contracts.Games.Queries;

namespace FCG.API.Endpoints;

public static class GamesEndpoints
{
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var GamesGroup = app.MapGroup("/Games");

        GamesGroup.MapGet("/", GetGamesAsync);
        GamesGroup.MapGet("/{key:guid}", GetGamesAsync);
        GamesGroup.MapPost("/", CreateGamesAsync);
        GamesGroup.MapPut("/{key:guid}", UpdateGamesAsync);
        GamesGroup.MapDelete("/{key:guid}", DeleteGamesAsync);
    }

    private static async Task<GamesQueryResponse> GetGamesAsync(
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(new GetGamesQueryRequest { Key = key }, cancellationToken);

    private static async Task<IReadOnlyCollection<GamesQueryResponse>> GetGamesAsync(
        [AsParameters] ListGamesQueryRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(request, cancellationToken);

    private static async Task<CreateGamesCommandResponse> CreateGamesAsync(
        [FromBody] CreateGamesCommandRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(request, cancellationToken);

    private static async Task<UpdateGamesCommandResponse> UpdateGamesAsync(
        [FromRoute] Guid key,
        [FromBody] UpdateGamesCommandRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        request.Key = key;
        return await mediator.Send(request, cancellationToken);
    }

    private static async Task DeleteGamesAsync(
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) =>  await mediator.Send(new DeleteGamesCommandRequest { Key = key }, cancellationToken);
}
