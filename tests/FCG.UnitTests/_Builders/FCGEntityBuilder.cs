using FCG.Domain.Catalogs;
using FCG.Domain.Users;

namespace FCG.UnitTests._Builders;
public class FCGEntityBuilder
{
    public Faker<User> User => new AutoFakerBase<User>()
        .RuleFor(u => u.Email, f => new Email(f.Internet.Email()))
        .RuleFor(u => u.PasswordHash, f => new Password("z15*^Popy8q}"));

    public Faker<Catalog> Catalog => new AutoFakerBase<Catalog>()
        .RuleFor(u => u.Name, f => f.Commerce.Categories(1).First())
        .RuleFor(u => u.Description, f => f.Commerce.ProductDescription())
        .Ignore(c => c.Games);
}
