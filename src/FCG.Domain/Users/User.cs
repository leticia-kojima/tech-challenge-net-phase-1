namespace FCG.Domain.Users;

public class User : EntityBase
{
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public ERole Role { get; private set; }
    public string Password { get; private set; }
}
