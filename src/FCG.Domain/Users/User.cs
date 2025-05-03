using FCG.Domain._Common;

namespace FCG.Domain.Users;

// TODO: Review this entity, this is just a demo!
public class User : EntityBase
{
    public User(string fistName, string lastName) : base()
    {
        FistName = fistName;
        LastName = lastName;
    }

    public string FistName { get; private set; }
    public string LastName { get; private set; }
}
