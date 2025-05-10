namespace FCG.Domain.Users;

public class User : EntityBase
{
    protected User() : base() { }

    public User(
        Guid key,
        string fullName,
        string email,
        ERole role,
        string passwordHash
    ) : base(key)
    {
        FullName = fullName;
        Email = email;
        Role = role;
        PasswordHash = passwordHash;
    }

    public string FullName { get; private set; }
    public string Email { get; private set; }
    public ERole Role { get; private set; }
    public string PasswordHash { get; private set; }
}
