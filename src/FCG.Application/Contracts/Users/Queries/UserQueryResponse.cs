using FCG.Application.Contracts.Users.Abstract;
using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Queries;
public class UserQueryResponse : UserCommandModelBase
{
    public UserQueryResponse(User user) : base(user)
    {
        Key = user.Key;
    }

    public Guid Key { get; set; }
}
