using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain.Users;

namespace FCG.Application.Commands.Users;
public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommandRequest, UpdateUserCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly IUserCommandRepository _userCommandRepository;

    public UpdateUserCommandHandler(
        IMediator mediator,
        IUserCommandRepository userCommandRepository
    )
    {
        _mediator = mediator;
        _userCommandRepository = userCommandRepository;
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

        var user = await _userCommandRepository.GetByIdAsync(request.Key, cancellationToken);

        if (user is null) throw new FCGNotFoundException(request.Key, nameof(User), $"User with key '{request.Key}' was not found.");

        var existUserWithSameEmail = await _userCommandRepository.ExistByEmailAsync(
            request.Email,
            request.Key,
            cancellationToken
        );
        if (existUserWithSameEmail) throw new FCGDuplicateException(nameof(User), $"The email '{request.Email}' is already in use.");

        user.SetData(request.FullName, request.Email);

        await _userCommandRepository.UpdateAsync(user, cancellationToken);

        await _mediator.Publish(new UserUpdatedEvent(user), cancellationToken);

        return new UpdateUserCommandResponse(user);
    }
}
