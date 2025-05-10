using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Abstract;
public abstract class UserCommandModelBase
{
    protected UserCommandModelBase()
    {        
    }

    protected UserCommandModelBase(User user)
    {
        // TODO: Update it
        //FirstName = user.FirstName;
        //LastName = user.LastName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}
