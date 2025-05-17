using FCG.Application.Contracts.Auth;
using FCG.Application.Contracts.Users.Commands;
using FCG.Domain._Common.Settings;

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
}