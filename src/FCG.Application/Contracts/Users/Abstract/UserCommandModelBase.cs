using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Abstract;
public abstract class UserCommandModelBase
{
    public UserCommandModelBase()
    {        
    }

    public UserCommandModelBase(User user)
    {
        FistName = user.FistName;
        LastName = user.LastName;
    }

    public string FistName { get; set; }
    public string LastName { get; set; }
}
