using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Queries;
using MediatR;

namespace FCG.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var usersGroup = app.MapGroup("/users");

        usersGroup.MapGet("/", GetUsersAsync);
        usersGroup.MapPost("/", CreateUserAsync);
    }

    private static async Task<IEnumerable<UserQueryResponse>> GetUsersAsync(
        ListUsersQueryRequest request,
        IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(request, cancellationToken);

    private static async Task<CreateUserCommandResponse> CreateUserAsync(
        CreateUserCommandRequest request,
        IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(request, cancellationToken);
}
