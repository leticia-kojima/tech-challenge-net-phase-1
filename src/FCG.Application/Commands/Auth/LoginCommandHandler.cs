using FCG.Application.Contracts.Auth;

namespace FCG.Application.Commands.Auth;
public class LoginCommandHandler : ICommandHandler<LoginCommandRequest, LoginCommandResponse>
{
    public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
