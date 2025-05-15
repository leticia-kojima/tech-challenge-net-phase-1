using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain._Common.Exceptions;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class DeleteUserCommandHandlerTests : TestHandlerBase<DeleteUserCommandHandler>
{
    private readonly IUserCommandRepository _repository;

    public DeleteUserCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _repository = GetMock<IUserCommandRepository>();
    }

    [Fact]
    public async Task ShouldDeleteAsync()
    {
        var user = _entityBuilder.User.Generate();
        var request = new DeleteUserCommandRequest { Key = user.Key };

        _repository.GetByIdAsync(user.Key, _cancellationToken)
            .Returns(user);

        await Handler.Handle(request, _cancellationToken);

        await _repository
            .Received(1)
            .DeleteAsync(
                Arg.Is<User>(x => x.Key == user.Key),
                _cancellationToken
            );
        await GetMock<IMediator>()
            .Received()
            .Publish(
                Arg.Any<UserDeletedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = new DeleteUserCommandRequest { Key = Guid.NewGuid() };

        _repository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as User);

        var exception = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        exception.Message.ShouldBe("User not found.");
    }
}
