using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Commands;
using FCG.Domain._Common.Exceptions;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class DeleteUserCommandHandlerTests : TestBase
{
    private readonly DeleteUserCommandHandler _handler;
    private readonly IUserCommandRepository _repository;

    public DeleteUserCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _repository = Substitute.For<IUserCommandRepository>();
        _handler = new DeleteUserCommandHandler();
    }

    [Fact]
    public async Task ShouldDeleteAsync()
    {
        var user = _entityBuilder.User.Generate();
        var request = new DeleteUserCommandRequest { Key = user.Key };
        
        _repository.GetByIdAsync(user.Key, _cancellationToken)
            .Returns(user);

        await _handler.Handle(request, _cancellationToken);

        await _repository
            .Received(1)
            .DeleteAsync(
                Arg.Is<User>(x => x.Key == user.Key),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = new DeleteUserCommandRequest { Key = Guid.NewGuid() };

        _repository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as User);

        var exception = await Assert.ThrowsAsync<FCGNotFoundException>(
            () => _handler.Handle(request, _cancellationToken)
        );

        Assert.Equal("User not found.", exception.Message);
    }
}
