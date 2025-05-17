using FCG.Domain.Users;

namespace FCG.UnitTests.Builders;
public class FCGEntityBuilder
{
    public Faker<User> User => new AutoFakerBase<User>()
        .RuleFor(u => u.Email, f => new Email(f.Internet.Email()))
        .RuleFor(u => u.PasswordHash, f => new Password("z15*^Popy8q}"));
}
