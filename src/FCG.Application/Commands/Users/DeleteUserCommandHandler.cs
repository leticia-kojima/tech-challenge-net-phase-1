using FCG.Application.Contracts.Users.Commands;

namespace FCG.Application.Commands.Users;
public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommandRequest>
{
    public Task Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
