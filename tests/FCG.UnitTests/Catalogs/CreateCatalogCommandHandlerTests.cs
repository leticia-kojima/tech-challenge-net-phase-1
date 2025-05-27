using FCG.Application.Commands.Catalogs;
using FCG.Application.Contracts.Catalogs.Events;
using FCG.Domain.Catalogs;

namespace FCG.UnitTests.Catalogs;
public class CreateCatalogCommandHandlerTests : TestHandlerBase<CreateCatalogCommandHandler>
{
    private readonly IMediator _mediator;
    private readonly ICatalogCommandRepository _catalogCommandRepository;

    public CreateCatalogCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _mediator = GetMock<IMediator>();
        _catalogCommandRepository = GetMock<ICatalogCommandRepository>();
    }

    [Fact]
    public async Task ShouldCreateCatalogAsync()
    {
        var request = _modelBuilder.CreateCatalogCommandRequest.Generate();

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Key.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe(request.Name);
        result.Description.ShouldBe(request.Description);
        await _catalogCommandRepository
            .Received(1)
            .AddAsync(
                Arg.Any<Catalog>(),
                _cancellationToken
            );
        await _mediator
            .Received(1)
            .Publish(
                Arg.Any<CatalogCreatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowExceptionForCatalogDuplicationAsync()
    {
        var request = _modelBuilder.CreateCatalogCommandRequest.Generate();

        _catalogCommandRepository.ExistByNameAsync(request.Name, cancellationToken: _cancellationToken)
            .Returns(true);

        var duplicateException = await Should.ThrowAsync<FCGDuplicateException>(
            () => Handler.Handle(request, _cancellationToken)
         );

        duplicateException.Message
            .ShouldBe($"The name '{request.Name}' is already in use.");
        await _catalogCommandRepository
            .DidNotReceive()
            .AddAsync(
                Arg.Any<Catalog>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<CatalogCreatedEvent>(),
                _cancellationToken
            );
    }
}
