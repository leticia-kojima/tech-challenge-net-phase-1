using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users;
public class CreateUserCommandResponse : UserCommandModelBase
{
    public CreateUserCommandResponse(User user)
    {
        Key = user.Key;
        FistName = user.FistName;
        LastName = user.LastName;
    }

    public Guid Key { get; set; }
}
