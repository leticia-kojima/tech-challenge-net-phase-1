using FCG.Application.Contracts.Users.Abstract;
using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Commands;
public class CreateUserCommandResponse : UserCommandModelBase
{
    public CreateUserCommandResponse(User user) : base(user)
    {
        Key = user.Key;
    }

    public Guid Key { get; set; }
}
