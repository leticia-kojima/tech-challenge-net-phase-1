using AutoBogus;
using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Commands;
using FCG.Domain.Users;
using FCG.UnitTests._Common;
using NSubstitute;

namespace FCG.UnitTests.Users;
public class CreateUserCommandHandlerTest : TestBase
{
    public CreateUserCommandHandlerTest(FCGFixture fixture) : base(fixture) { }

    [Fact]
    // This is a demo unit test!
    public async Task ShouldCreateUserAsync()
    {
        var request = AutoFaker.Generate<CreateUserCommandRequest>();
        var repository = Substitute.For<IUserCommandRepository>();
        var command = new CreateUserCommandHandler(repository);

        var result = await command.Handle(request, _cancellationToken);

        Assert.NotEqual(Guid.Empty, result.Key);
        Assert.Equal(request.FistName, result.FistName);
        Assert.Equal(request.LastName, result.LastName);
        await repository
            .Received(1)
            .AddAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
    }
}
