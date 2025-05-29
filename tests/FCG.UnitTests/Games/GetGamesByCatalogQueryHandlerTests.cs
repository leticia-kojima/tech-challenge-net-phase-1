using FCG.Application.Contracts.Games.Queries;
using FCG.Domain.Catalogs;

namespace FCG.UnitTests.Catalogs;

public class GetGamesByCatalogQueryHandlerTests : TestHandlerBase<GetGamesByCatalogQueryHandler>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public GetGamesByCatalogQueryHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _catalogQueryRepository = GetMock<ICatalogQueryRepository>();
    }

    [Fact]
    public async Task ShouldGetListAsync()
    {
        var catalog = _entityBuilder.CatalogWithGames(3).Generate();
        var request = new GetGamesByCatalogQueryRequest { CatalogKey = catalog.Key };

        _catalogQueryRepository.GetByIdAsync(catalog.Key, _cancellationToken)
            .Returns(catalog);

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(catalog.Games.Count);
        foreach (var game in catalog.Games)
        {
            var response = result.SingleOrDefault(r => r.Key == game.Key);
            response.ShouldNotBeNull();
            response.Key.ShouldBe(game.Key);
            response.Title.ShouldBe(game.Title);
            response.Description.ShouldBe(game.Description);
            response.CatalogKey.ShouldBe(catalog.Key);
            response.CatalogName.ShouldBe(catalog.Name);
        }
    }
}
