using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain.Users;

namespace FCG.Application.Commands.Users;
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly IUserCommandRepository _userCommandRepository;

    public CreateUserCommandHandler(
        IMediator mediator,
        IUserCommandRepository userCommandRepository
    )
    {
        _mediator = mediator;
        _userCommandRepository = userCommandRepository;
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

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new FCGValidationException(
                nameof(request.Password),
                $"{nameof(request.Password)} is required."
            );

        var existUserWithSameEmail = await _userCommandRepository.ExistByEmailAsync(request.Email, cancellationToken: cancellationToken);
        if (existUserWithSameEmail) throw new FCGDuplicateException(nameof(User), $"The email '{request.Email}' is already in use.");

        var user = new User(
            Guid.NewGuid(),
            request.FullName,
            new Email(request.Email),
            ERole.User,
            new Password(request.Password)
        );

        await _userCommandRepository.AddAsync(user, cancellationToken);

        await _mediator.Publish(new UserCreatedEvent(user), cancellationToken);

        return new CreateUserCommandResponse(user);
    }
}
