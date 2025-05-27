using FCG.Application.Contracts.Catalogs.Queries;
using FCG.Application.Queries.Catalogs;
using FCG.Domain.Catalogs;

namespace FCG.UnitTests.Catalogs;

public class ListCatalogsQueryHandlerTests : TestHandlerBase<ListCatalogsQueryHandler>
{
    private readonly ICatalogQueryRepository _catalogQueryRepository;

    public ListCatalogsQueryHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _catalogQueryRepository = GetMock<ICatalogQueryRepository>();
    }

    [Fact]
    public async Task ShouldGetListAsync()
    {        
        var catalogName = _faker.Commerce.Categories(1).First();
        var request = new ListCatalogsQueryRequest { Search = catalogName};
        var catalogs = _entityBuilder.Catalog
            .RuleFor(u => u.Name, catalogName)
            .Generate(5);

        _catalogQueryRepository.SearchAsync(request.Search, _cancellationToken)
            .Returns(catalogs);

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(catalogs.Count);
    }
}
