using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Abstract;
public abstract class UserCommandModelBase
{
    protected UserCommandModelBase()
    {        
    }

    protected UserCommandModelBase(User user)
    {
        FirstName = user.FirstName;
        LastName = user.LastName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}
