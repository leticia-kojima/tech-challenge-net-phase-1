using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain.Users;

namespace FCG.Application.Commands.Users;
public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommandRequest>
{
    private readonly IMediator _mediator;
    private readonly IUserCommandRepository _userCommandRepository;

    public DeleteUserCommandHandler(
        IMediator mediator,
        IUserCommandRepository userCommandRepository
    )
    {
        _mediator = mediator;
        _userCommandRepository = userCommandRepository;
    }

    public async Task Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _userCommandRepository.GetByIdAsync(request.Key, cancellationToken);

        if (user is null) throw new FCGNotFoundException(request.Key, nameof(User), $"User with key '{request.Key}' was not found.");

        await _userCommandRepository.DeleteAsync(user, cancellationToken);
        
        await _mediator.Publish(new UserDeletedEvent(user), cancellationToken);
    }
}
