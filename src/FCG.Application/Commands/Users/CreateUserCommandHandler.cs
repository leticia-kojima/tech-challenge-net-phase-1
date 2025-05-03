using FCG.Application._Common;
using FCG.Application.Contracts.Users.Commands;
using FCG.Domain.Users;

namespace FCG.Application.Commands.Users;
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    private readonly IUserCommandRepository _userCommandRepository;

    public CreateUserCommandHandler(IUserCommandRepository userCommandRepository)
    {
        _userCommandRepository = userCommandRepository;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = new User(request.FistName, request.LastName);

        await _userCommandRepository.AddAsync(user, cancellationToken);

        return new CreateUserCommandResponse(user);
    }
}
