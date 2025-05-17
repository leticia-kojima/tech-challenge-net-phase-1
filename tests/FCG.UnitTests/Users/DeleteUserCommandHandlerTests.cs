using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Commands;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class DeleteUserCommandHandlerTests : TestHandlerBase<DeleteUserCommandHandler>
{
    private readonly IMediator _mediator;
    private readonly IUserCommandRepository _userCommandRepository;

    public DeleteUserCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _mediator = GetMock<IMediator>();
        _userCommandRepository = GetMock<IUserCommandRepository>();
    }

    [Fact]
    public async Task ShouldDeleteAsync()
    {
        var user = _entityBuilder.User.Generate();
        var request = new DeleteUserCommandRequest { Key = user.Key };

        _userCommandRepository.GetByIdAsync(user.Key, _cancellationToken)
            .Returns(user);

        await Handler.Handle(request, _cancellationToken);

        await _userCommandRepository
            .Received(1)
            .DeleteAsync(
                Arg.Is<User>(x => x.Key == user.Key),
                _cancellationToken
            );
        await _mediator
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

        _userCommandRepository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as User);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        notFoundException.Message.ShouldBe($"User with key '{request.Key}' was not found.");
    }
}
