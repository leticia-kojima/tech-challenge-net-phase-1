using FCG.Application.Contracts.Auth;

namespace FCG.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth");

        authGroup.MapPost("/login", LoginAsync)
            .AllowAnonymous();
    }

    private static async Task<LoginCommandResponse> LoginAsync(
        [FromBody] LoginCommandRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(request, cancellationToken);
}
