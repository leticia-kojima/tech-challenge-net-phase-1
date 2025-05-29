using FCG.Application.Contracts.Auth;
using FCG.Application.Contracts.Catalogs.Commands;
using FCG.Application.Contracts.Games.Commands;
using FCG.Application.Contracts.Games.Queries;
using FCG.Application.Contracts.Users.Commands;
using FCG.Domain.Games;

namespace FCG.UnitTests._Builders;
public class FCGModelBuilder
{
    public Faker<CreateUserCommandRequest> CreateUserCommandRequest
        => new AutoFakerBase<CreateUserCommandRequest>()
        .RuleFor(u => u.Password, "z15*^Popy8q}");

    public Faker<UpdateUserCommandRequest> UpdateUserCommandRequest
        => new AutoFakerBase<UpdateUserCommandRequest>();

    public Faker<LoginCommandRequest> LoginCommandRequest
        => new AutoFakerBase<LoginCommandRequest>()
        .RuleFor(u => u.Password, "ZG9nH;#5lWw3&")
        .RuleFor(u => u.Email, f => f.Internet.Email());

    public Faker<CreateCatalogCommandRequest> CreateCatalogCommandRequest
        => new AutoFakerBase<CreateCatalogCommandRequest>();

    public Faker<UpdateCatalogCommandRequest> UpdateCatalogCommandRequest
        => new AutoFakerBase<UpdateCatalogCommandRequest>();

    public Faker<CreateGameCommandRequest> CreateGameCommandRequest
        => new AutoFakerBase<CreateGameCommandRequest>();

    public Faker<UpdateGameCommandRequest> UpdateGameCommandRequest
        => new AutoFakerBase<UpdateGameCommandRequest>();

    public Faker<UpdateGameCommandRequest> UpdateGameCommandRequestFromGame(Game game)
        => new AutoFakerBase<UpdateGameCommandRequest>()
        .RuleFor(x => x.Key, game.Key)
        .RuleFor(x => x.CatalogKey, game.CatalogKey);

    public Faker<DeleteGameCommandRequest> DeleteGameCommandRequest
        => new AutoFakerBase<DeleteGameCommandRequest>();

    public Faker<DeleteGameCommandRequest> DeleteGameCommandRequestFromGame(Game game) => DeleteGameCommandRequest
        .RuleFor(x => x.Key, game.Key)
        .RuleFor(x => x.CatalogKey, game.CatalogKey);

    public Faker<GetGameByKeyQueryRequest> GetGameByKeyQueryRequest
        => new AutoFakerBase<GetGameByKeyQueryRequest>();

    public Faker<GetGameByKeyQueryRequest> GetGameByKeyQueryRequestFromGame(Game game) => GetGameByKeyQueryRequest
        .RuleFor(x => x.Key, game.Key)
        .RuleFor(x => x.CatalogKey, game.CatalogKey);
}