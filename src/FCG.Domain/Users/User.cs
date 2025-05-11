using FCG.Domain.ValueObjects;

namespace FCG.Domain.Users;

public class User : EntityBase
{
    protected User() : base() { }

    public User(
        Guid key,
        string fullName,
        Email email,  
        ERole role,
        Password passwordHash
    ) : base(key)
    {
        FullName = fullName;
        Email = email;
        Role = role;
        PasswordHash = passwordHash;
    }

    public string FullName { get; private set; }
    public Email Email { get; private set; }
    public ERole Role { get; private set; }
    public Password PasswordHash { get; private set; }
}
