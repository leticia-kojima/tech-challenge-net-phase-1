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
        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new FCGValidationException(
                nameof(request.FullName),
                $"{nameof(request.FullName)} is required."
            );

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new FCGValidationException(
                nameof(request.Email),
                $"{nameof(request.Email)} is required."
            );

        if (string.IsNullOrWhiteSpace(request.Role))
            throw new FCGValidationException(
                nameof(request.Role),
                $"{nameof(request.Role)} is required."
            );

        if (string.IsNullOrWhiteSpace(request.PasswordHash))
            throw new FCGValidationException(
                nameof(request.PasswordHash),
                $"{nameof(request.PasswordHash)} is required."
            );

        // TODO: Set the properties of the user entity based on the request
        var user = new User(
            Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
            PasswordHash = request.PasswordHash
        );

        await _userCommandRepository.AddAsync(user, cancellationToken);

        await _mediator.Publish(new UserCreatedEvent(user), cancellationToken);

        return new CreateUserCommandResponse(user);
    }
}
