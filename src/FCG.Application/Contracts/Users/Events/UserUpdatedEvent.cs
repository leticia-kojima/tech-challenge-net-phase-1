using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Events;
public class UserUpdatedEvent : IEvent
{
    public UserUpdatedEvent(User user)
    {
        User = user;
    }

    public User User { get; }
}
