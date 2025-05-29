using FCG.Application.Commands.Games;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Games;

namespace FCG.UnitTests.Games;
public class DeleteGameCommandHandlerTests : TestHandlerBase<DeleteGameCommandHandler>
{
    private readonly IMediator _mediator;
    private readonly IGameCommandRepository _gameCommandRepository;

    public DeleteGameCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _mediator = GetMock<IMediator>();
        _gameCommandRepository = GetMock<IGameCommandRepository>();
    }

    [Fact]
    public async Task ShouldDeleteAsync()
    {
        var game = _entityBuilder.Game.Generate();
        var request = _modelBuilder.DeleteGameCommandRequestFromGame(game)
            .Generate();

        _gameCommandRepository.GetByIdAsync(game.Key, _cancellationToken)
            .Returns(game);

        await Handler.Handle(request, _cancellationToken);

        await _gameCommandRepository
            .Received(1)
            .DeleteAsync(
                Arg.Is<Game>(x => x.Key == game.Key),
                _cancellationToken
            );
        await _mediator
            .Received()
            .Publish(
                Arg.Any<GameDeletedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = _modelBuilder.DeleteGameCommandRequest
            .Generate();

        _gameCommandRepository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as Game);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)        
        );

        notFoundException.Message.ShouldBe($"Game with key '{request.Key}' was not found.");
    }


    [Fact]
    public async Task ShouldThrowExceptionWhenGameDoesNotBelongToCatalogAsync()
    {
        var game = _entityBuilder.Game.Generate();
        var request = _modelBuilder.DeleteGameCommandRequestFromGame(game)
            .RuleFor(x => x.CatalogKey, Guid.NewGuid())
            .Generate();

        _gameCommandRepository.GetByIdAsync(game.Key, _cancellationToken).Returns(game);

        var validationException = await Should.ThrowAsync<FCGValidationException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        validationException.Message
            .ShouldBe($"Game with key '{request.Key}' does not belong to catalog with key '{request.CatalogKey}'.");
        await _gameCommandRepository
            .DidNotReceive()
            .DeleteAsync(
                Arg.Any<Game>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<GameDeletedEvent>(),
                _cancellationToken
            );
    }
}
