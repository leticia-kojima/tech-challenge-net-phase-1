using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Events;
public class UserDeletedEvent : IEvent
{
    public UserDeletedEvent(User user)
    {
        User = user;
    }

    public User User { get; }
}
