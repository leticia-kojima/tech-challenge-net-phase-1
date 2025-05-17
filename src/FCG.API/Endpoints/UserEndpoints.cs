using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Queries;

namespace FCG.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var usersGroup = app.MapGroup("/users");

        usersGroup.MapGet("/", GetUsersAsync);
        usersGroup.MapGet("/{key:guid}", GetUserAsync);
        usersGroup.MapPost("/", CreateUserAsync);
        usersGroup.MapPut("/{key:guid}", UpdateUserAsync);
        usersGroup.MapDelete("/{key:guid}", DeleteUserAsync);
    }

    private static async Task<UserQueryResponse> GetUserAsync(
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(new GetUserQueryRequest { Key = key }, cancellationToken);

    private static async Task<IReadOnlyCollection<UserQueryResponse>> GetUsersAsync(
        [AsParameters] ListUsersQueryRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(request, cancellationToken);

    private static async Task<CreateUserCommandResponse> CreateUserAsync(
        [FromBody] CreateUserCommandRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(request, cancellationToken);

    private static async Task<UpdateUserCommandResponse> UpdateUserAsync(
        [FromRoute] Guid key,
        [FromBody] UpdateUserCommandRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        request.Key = key;
        return await mediator.Send(request, cancellationToken);
    }

    private static async Task DeleteUserAsync(
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) =>  await mediator.Send(new DeleteUserCommandRequest { Key = key }, cancellationToken);
}
