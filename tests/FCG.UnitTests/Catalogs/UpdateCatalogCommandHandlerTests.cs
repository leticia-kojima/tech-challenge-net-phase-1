using FCG.Application.Commands.Catalogs;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;

namespace FCG.UnitTests.Catalogs;
public class UpdateCatalogCommandHandlerTests : TestHandlerBase<UpdateCatalogCommandHandler>
{
    private readonly IMediator _mediator;
    private readonly ICatalogCommandRepository _catalogCommandRepository;

    public UpdateCatalogCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _mediator = GetMock<IMediator>();
        _catalogCommandRepository = GetMock<ICatalogCommandRepository>();
    }

    [Fact]
    public async Task ShouldUpdateCatalogAsync()
    {
        var catalog = _entityBuilder.Catalog.Generate();
        var request = _modelBuilder.UpdateCatalogCommandRequest
            .RuleFor(c => c.Key, catalog.Key)
            .Generate();

        _catalogCommandRepository.GetByIdAsync(catalog.Key, _cancellationToken).Returns(catalog);

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Key.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe(catalog.Name);
        result.Description.ShouldBe(catalog.Description);
        await _catalogCommandRepository
            .Received(1)
            .UpdateAsync(
                Arg.Is<Catalog>(c => c.Key == request.Key
                    && c.Name == request.Name
                    && c.Description == request.Description),
                _cancellationToken
            );
        await _mediator
            .Received(1)
            .Publish(
                Arg.Any<CatalogUpdatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = _modelBuilder.UpdateCatalogCommandRequest
            .Generate();

        _catalogCommandRepository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as Catalog);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        notFoundException.Message
            .ShouldBe($"Catalog with key '{request.Key}' was not found.");
        await _catalogCommandRepository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<Catalog>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<CatalogUpdatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowExceptionForCatalogDuplicationAsync()
    {
        var catalog = _entityBuilder.Catalog.Generate();
        var request = _modelBuilder.UpdateCatalogCommandRequest
            .RuleFor(c => c.Key, catalog.Key)
            .Generate();

        _catalogCommandRepository.GetByIdAsync(catalog.Key, _cancellationToken).Returns(catalog);
        _catalogCommandRepository.ExistByNameAsync(request.Name, request.Key, _cancellationToken)
            .Returns(true);

        var duplicateException = await Should.ThrowAsync<FCGDuplicateException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        duplicateException.Message
            .ShouldBe($"The name '{request.Name}' is already in use.");
        await _catalogCommandRepository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<Catalog>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<CatalogUpdatedEvent>(),
                _cancellationToken
            );
    }
}
