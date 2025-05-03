using FCG.Application.Contracts.Users;
using MediatR;

namespace FCG.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var usersGroup = app.MapGroup("/users");

        usersGroup.MapPost("/", CreateUserAsync);
    }

    private static async Task<CreateUserCommandResponse> CreateUserAsync(
        CreateUserCommandRequest request,
        IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(request, cancellationToken);
}
