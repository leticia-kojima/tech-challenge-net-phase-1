using FCG.Domain.Users;

namespace FCG.Application.Contracts.Users.Abstract;
public abstract class UserCommandModelBase
{
    protected UserCommandModelBase()
    {        
    }

    protected UserCommandModelBase(User user)
    {

        FullName = user.FullName;
        Email = user.Email;
        Role = user.Role;
        PasswordHash = user.PasswordHash;

    }

    public string FullName { get; set; }
    public string Email { get; set; }
    public ERole Role { get; set; }
    public string PasswordHash { get; set; }

}
