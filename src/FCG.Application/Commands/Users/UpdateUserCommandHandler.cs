using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain._Common.Exceptions;
using FCG.Domain.Users;

namespace FCG.Application.Commands.Users;
public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
{
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IMediator _mediator;

    public UpdateUserCommandHandler(
        IUserCommandRepository userCommandRepository,
        IMediator mediator
    )
    {
        _userCommandRepository = userCommandRepository;
        _mediator = mediator;
    }

    public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
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

        if (!Enum.IsDefined(request.Role))
            throw new FCGValidationException(
                nameof(request.Role),
                $"{nameof(request.Role)} is required."
            );

        var user = await _userCommandRepository.GetByIdAsync(request.Key, cancellationToken);

        if (user is null) throw new FCGNotFoundException(request.Key, nameof(User), "User not found.");

        var existUserWithSameEmail = await _userCommandRepository.ExistByEmailAsync(
            request.Email,
            request.Key,
            cancellationToken
        );
        if (existUserWithSameEmail) throw new FCGDuplicateException(nameof(User), "An user with this email already exists.");

        user.SetData(request.FullName, request.Email, request.Role);

        await _userCommandRepository.UpdateAsync(user, cancellationToken);

        await _mediator.Publish(new UserUpdatedEvent(user), cancellationToken);

        return new UpdateUserCommandResponse(user);
    }
}
