using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain._Common.Exceptions;
using FCG.Domain.Users;

namespace FCG.Application.Commands.Users;
public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommandRequest>
{
    private readonly IMediator _mediator;
    private readonly IUserCommandRepository _repository;

    public DeleteUserCommandHandler(IMediator mediator, IUserCommandRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Key, cancellationToken);
        
        if (user is null) throw new FCGNotFoundException(request.Key, nameof(User), "User not found.");
        
        await _repository.DeleteAsync(user, cancellationToken);
        
        await _mediator.Publish(new UserDeletedEvent(user), cancellationToken);
    }
}
