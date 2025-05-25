using FCG.Application.Contracts.Catalogs.Queries;
using FCG.Application.Queries.Catalogs;
using FCG.Domain.Catalogs;

namespace FCG.UnitTests.Catalogs;

public class GetCatalogQueryHandlerTests : TestHandlerBase<GetCatalogQueryHandler>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;
    public GetCatalogQueryHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _catalogQueryRepository = GetMock<ICatalogQueryRepository>();
    }

    [Fact]
    public async Task ShouldGetAsync()
    {
        var catalog = _entityBuilder.Catalog.Generate();
        var request = new GetCatalogQueryRequest { Key = catalog.Key };

        _catalogQueryRepository.GetByIdAsync(catalog.Key, _cancellationToken)
            .Returns(catalog);

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Key.ShouldBe(catalog.Key);
        result.Name.ShouldBe(catalog.Name);
        result.Description.ShouldBe(catalog.Description);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = new GetCatalogQueryRequest { Key = Guid.NewGuid() };
        _catalogQueryRepository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as Catalog);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        notFoundException.Message.ShouldBe($"Catalog with key '{request.Key}' was not found.");

    }
}
