using FCG.Application.Contracts.Users.Events;
using FCG.Domain.Users;

namespace FCG.Application.Events.Users;
public class UserDeletedEventHandler : IEventHandler<UserDeletedEvent>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public UserDeletedEventHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _userQueryRepository.DeleteAsync(notification.User, cancellationToken);
    }
}
