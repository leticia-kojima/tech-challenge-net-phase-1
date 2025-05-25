using FCG.Application.Commands.Catalogs;
using FCG.Application.Contracts.Catalogs.Commands;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;

namespace FCG.UnitTests.Catalogs;
public class DeleteCatalogCommandHandlerTests : TestHandlerBase<DeleteCatalogCommandHandler>
{
    private readonly IMediator _mediator;
    private readonly ICatalogCommandRepository _catalogCommandRepository;
    public DeleteCatalogCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _mediator = GetMock<IMediator>();
        _catalogCommandRepository = GetMock<ICatalogCommandRepository>();
    }

    [Fact]
    public async Task ShouldDeleteAsync()
    {
        var catalog = _entityBuilder.Catalog.Generate();
        var request = new DeleteCatalogCommandRequest { Key = catalog.Key };

        _catalogCommandRepository.GetByIdAsync(catalog.Key, _cancellationToken)
            .Returns(catalog);

        await Handler.Handle(request, _cancellationToken);

        await _catalogCommandRepository
            .Received(1)
            .DeleteAsync(
                Arg.Is<Catalog>(x => x.Key == catalog.Key),
                _cancellationToken
            );
        await _mediator
            .Received()
            .Publish(
                Arg.Any<CatalogDeletedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = new DeleteCatalogCommandRequest { Key = Guid.NewGuid() };

        _catalogCommandRepository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as Catalog);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)        
        );

        notFoundException.Message.ShouldBe($"Catalog with key '{request.Key}' was not found.");
    }
}
