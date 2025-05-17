using FCG.Application.Contracts.Users.Abstract;
using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Commands;
public class UpdateUserCommandResponse : UserCommandModelBase
{
    public UpdateUserCommandResponse(User user) : base(user)
    {
        Key = user.Key;
    }

    public Guid Key { get; set; }
}
