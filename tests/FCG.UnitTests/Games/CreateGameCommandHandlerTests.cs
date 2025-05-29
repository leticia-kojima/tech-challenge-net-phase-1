using FCG.Application.Commands.Games;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Games;

namespace FCG.UnitTests.Games;
public class CreateGameCommandHandlerTests : TestHandlerBase<CreateGameCommandHandler>
{
    private readonly IMediator _mediator;
    private readonly IGameCommandRepository _gameCommandRepository;

    public CreateGameCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _mediator = GetMock<IMediator>();
        _gameCommandRepository = GetMock<IGameCommandRepository>();
    }

    [Fact]
    public async Task ShouldCreateGameAsync()
    {
        var request = _modelBuilder.CreateGameCommandRequest.Generate();

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Key.ShouldNotBe(Guid.Empty);
        result.Title.ShouldBe(request.Title);
        result.Description.ShouldBe(request.Description);
        await _gameCommandRepository
            .Received(1)
            .AddAsync(
                Arg.Any<Game>(),
                _cancellationToken
            );
        await _mediator
            .Received(1)
            .Publish(
                Arg.Any<GameCreatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowExceptionForGameDuplicationAsync()
    {
        var request = _modelBuilder.CreateGameCommandRequest.Generate();

        _gameCommandRepository.ExistByTitleAsync(request.Title, cancellationToken: _cancellationToken)
            .Returns(true);

        var duplicateException = await Should.ThrowAsync<FCGDuplicateException>(
            () => Handler.Handle(request, _cancellationToken)
         );

        duplicateException.Message
            .ShouldBe($"The title '{request.Title}' is already in use.");
        await _gameCommandRepository
            .DidNotReceive()
            .AddAsync(
                Arg.Any<Game>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<GameCreatedEvent>(),
                _cancellationToken
            );
    }
}
