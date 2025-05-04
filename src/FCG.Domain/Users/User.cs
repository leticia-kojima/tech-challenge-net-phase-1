using FCG.Domain._Common;

namespace FCG.Domain.Users;

// TODO: Review this entity, this is just a demo!
public class User : EntityBase
{
    public User(string firstName, string lastName) : base()
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
}
