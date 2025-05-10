using FCG.Application.Contracts.Users.Events;
using FCG.Domain.Users;

namespace FCG.Application.Events.Users;
public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public UserCreatedEventHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _userQueryRepository.AddAsync(notification.User, cancellationToken);
    }
}
