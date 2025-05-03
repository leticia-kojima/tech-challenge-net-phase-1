using FCG.Application.Contracts.Users;
using FCG.Domain.Users;
using MediatR;

namespace FCG.Application.Commands.Users;
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
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
