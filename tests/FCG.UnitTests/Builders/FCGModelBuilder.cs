using FCG.Application.Contracts.Catalogs.Commands;
using FCG.Application.Contracts.Users.Commands;

namespace FCG.UnitTests.Builders;
public class FCGModelBuilder
{
    public Faker<CreateUserCommandRequest> CreateUserCommandRequest
        => new AutoFakerBase<CreateUserCommandRequest>()
        .RuleFor(u => u.Password, "z15*^Popy8q}");

    public Faker<UpdateUserCommandRequest> UpdateUserCommandRequest
        => new AutoFakerBase<UpdateUserCommandRequest>();

    public Faker<CreateCatalogCommandRequest> CreateCatalogCommandRequest
        => new AutoFakerBase<CreateCatalogCommandRequest>();

    public Faker<UpdateCatalogCommandRequest> UpdateCatalogCommandRequest
        => new AutoFakerBase<UpdateCatalogCommandRequest>();
}