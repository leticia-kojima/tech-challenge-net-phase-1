using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain._Common;
using FCG.Domain.Users;

namespace FCG.Application.Commands.Users;
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IMediator _mediator;

    public CreateUserCommandHandler(
        IUserCommandRepository userCommandRepository,
        IMediator mediator
    )
    {
        _userCommandRepository = userCommandRepository;
        _mediator = mediator;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
            throw new FCGValidationException(
                nameof(request.FirstName),
                $"{nameof(request.FirstName)} is required."
            );

        if (string.IsNullOrWhiteSpace(request.LastName))
            throw new FCGValidationException(
                nameof(request.LastName),
                $"{nameof(request.LastName)} is required."
            );

        // TODO: Set the properties of the user entity based on the request
        var user = new User();

        await _userCommandRepository.AddAsync(user, cancellationToken);

        await _mediator.Publish(new UserCreatedEvent(user), cancellationToken);

        return new CreateUserCommandResponse(user);
    }
}
