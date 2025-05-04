using FCG.Domain.Users;
using MediatR;

namespace FCG.Application.Contracts.Users.Events;
public class UserCreatedEvent : INotification
{
    public UserCreatedEvent(User user)
    {
        User = user;
    }

    public User User { get; }
}
