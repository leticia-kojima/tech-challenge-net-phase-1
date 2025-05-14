using Bogus;
using FCG.Application.Contracts.Users.Commands;

namespace FCG.UnitTests.Builders;
public class FCGModelBuilder
{
    public Faker<CreateUserCommandRequest> CreateUserCommandRequest
        => new AutoFakerBase<CreateUserCommandRequest>()
        .RuleFor(u => u.Password, "z15*^Popy8q}");

    public Faker<UpdateUserCommandRequest> UpdateUserCommandRequest
        => new AutoFakerBase<UpdateUserCommandRequest>();
}