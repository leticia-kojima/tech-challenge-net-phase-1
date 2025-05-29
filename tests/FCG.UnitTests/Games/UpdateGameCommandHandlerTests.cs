using FCG.Application.Commands.Games;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Games;

namespace FCG.UnitTests.Games;
public class UpdateGameCommandHandlerTests : TestHandlerBase<UpdateGameCommandHandler>
{
    private readonly IMediator _mediator;
    private readonly IGameCommandRepository _gameCommandRepository;

    public UpdateGameCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _mediator = GetMock<IMediator>();
        _gameCommandRepository = GetMock<IGameCommandRepository>();
    }

    [Fact]
    public async Task ShouldUpdateGameAsync()
    {
        var game = _entityBuilder.Game.Generate();
        var request = _modelBuilder.UpdateGameCommandRequestFromGame(game)
            .Generate();

        _gameCommandRepository.GetByIdAsync(game.Key, _cancellationToken).Returns(game);

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Key.ShouldNotBe(Guid.Empty);
        result.Title.ShouldBe(game.Title);
        result.Description.ShouldBe(game.Description);
        await _gameCommandRepository
            .Received(1)
            .UpdateAsync(
                Arg.Is<Game>(c => c.Key == request.Key
                    && c.Title == request.Title
                    && c.Description == request.Description),
                _cancellationToken
            );
        await _mediator
            .Received(1)
            .Publish(
                Arg.Any<GameUpdatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = _modelBuilder.UpdateGameCommandRequest
            .Generate();

        _gameCommandRepository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as Game);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        notFoundException.Message
            .ShouldBe($"Game with key '{request.Key}' was not found.");
        await _gameCommandRepository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<Game>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<GameUpdatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowExceptionForGameDuplicationAsync()
    {
        var game = _entityBuilder.Game.Generate();
        var request = _modelBuilder.UpdateGameCommandRequestFromGame(game)
            .Generate();

        _gameCommandRepository.GetByIdAsync(game.Key, _cancellationToken).Returns(game);
        _gameCommandRepository.ExistByTitleAsync(request.Title, request.Key, _cancellationToken)
            .Returns(true);

        var duplicateException = await Should.ThrowAsync<FCGDuplicateException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        duplicateException.Message
            .ShouldBe($"The title '{request.Title}' is already in use.");
        await _gameCommandRepository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<Game>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<GameUpdatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenGameDoesNotBelongToCatalogAsync()
    {
        var game = _entityBuilder.Game.Generate();
        var request = _modelBuilder.UpdateGameCommandRequestFromGame(game)
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
            .UpdateAsync(
                Arg.Any<Game>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<GameUpdatedEvent>(),
                _cancellationToken
            );
    }
}
