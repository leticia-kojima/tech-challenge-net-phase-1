using FCG.Domain.Catalogs;
using FCG.Domain.Games;
using FCG.Domain.Users;

namespace FCG.UnitTests._Builders;
public class FCGEntityBuilder
{
    public Faker<User> User => new AutoFakerBase<User>()
        .RuleFor(u => u.Email, f => new Email(f.Internet.Email()))
        .RuleFor(u => u.PasswordHash, f => new Password("z15*^Popy8q}"));

    public Faker<Catalog> Catalog => new AutoFakerBase<Catalog>()
        .RuleFor(c => c.Name, f => f.Commerce.Categories(1).First())
        .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
        .Ignore(c => c.Games);

    public Faker<Catalog> CatalogWithGames(int count = 1)
    {
        var key = Guid.NewGuid();
        var gameBuilder = Game
            .RuleFor(g => g.CatalogKey, key);

        return Catalog
            .RuleFor(c => c.Key, key)
            .RuleFor(c => c.Games, gameBuilder.Generate(count));
    }

    public Faker<Game> Game => new AutoFakerBase<Game>()
        .Ignore(c => c.Evaluations)
        .Ignore(c => c.Downloads)
        .Ignore(c => c.Catalog);
}
