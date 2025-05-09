using FCG.Application._Common.Contracts;
using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Events;
public class UserCreatedEvent : IEvent
{
    public UserCreatedEvent(User user)
    {
        User = user;
    }

    public User User { get; }
}
