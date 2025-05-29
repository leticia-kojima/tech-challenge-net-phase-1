using FCG.Application.Queries.Games;
using FCG.Domain.Catalogs;

namespace FCG.UnitTests.Games;

public class GetGameByKeyQueryHandlerTests : TestHandlerBase<GetGameByKeyQueryHandler>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public GetGameByKeyQueryHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _catalogQueryRepository = GetMock<ICatalogQueryRepository>();
    }

    [Fact]
    public async Task ShouldGetAsync()
    {
        var catalog = _entityBuilder.CatalogWithGames(1).Generate();
        var game = catalog.Games.ElementAt(0);
        var request = _modelBuilder.GetGameByKeyQueryRequestFromGame(game)
            .Generate();

        _catalogQueryRepository.GetByIdAsync(catalog.Key, _cancellationToken)
            .Returns(catalog);

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Key.ShouldBe(game.Key);
        result.Title.ShouldBe(game.Title);
        result.Description.ShouldBe(game.Description);
        result.CatalogKey.ShouldBe(catalog.Key);
        result.CatalogName.ShouldBe(catalog.Name);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = _modelBuilder.GetGameByKeyQueryRequest.Generate();
        _catalogQueryRepository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as Catalog);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        notFoundException.Message.ShouldBe($"Catalog with key '{request.CatalogKey}' was not found.");
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionWhenGameDoesNotExistInCatalogAsync()
    {
        var catalog = _entityBuilder.Catalog.Generate();
        var request = _modelBuilder.GetGameByKeyQueryRequest
            .RuleFor(x => x.CatalogKey, catalog.Key)
            .Generate();

        _catalogQueryRepository.GetByIdAsync(catalog.Key, _cancellationToken)
            .Returns(catalog);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        notFoundException.Message.ShouldBe($"Game with key '{request.Key}' does not exist in the catalog.");
    }
}
