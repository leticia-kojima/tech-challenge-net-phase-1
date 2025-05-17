using FCG.Application.Contracts.Users.Events;
using FCG.Domain.Users;

namespace FCG.Application.Events.Users;
public class UserUpdatedEventHandler : IEventHandler<UserUpdatedEvent>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public UserUpdatedEventHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _userQueryRepository.UpdateAsync(notification.User, cancellationToken);
    }
}
