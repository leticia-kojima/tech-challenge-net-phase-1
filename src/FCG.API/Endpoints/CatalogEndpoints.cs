using FCG.Application.Contracts.Catalogs.Commands;
using FCG.Application.Contracts.Catalogs.Queries;

namespace FCG.API.Endpoints;

public static class CatalogEndpoints
{
    public static void MapCatalogEndpoints(this WebApplication app)
    {
        var catalogsGroup = app.MapGroup("/catalogs");

        catalogsGroup.MapGet("/", GetCatalogsAsync);
        catalogsGroup.MapGet("/{key:guid}", GetCatalogAsync);
        catalogsGroup.MapPost("/", CreateCatalogAsync);
        catalogsGroup.MapPut("/{key:guid}", UpdateCatalogAsync);
        catalogsGroup.MapDelete("/{key:guid}", DeleteCatalogAsync);
    }

    private static async Task<CatalogQueryResponse> GetCatalogAsync(
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancelattionToken
    ) => await mediator.Send(new GetCatalogQueryRequest { Key = key }, cancelattionToken);

    private static async Task<IReadOnlyCollection<CatalogQueryResponse>> GetCatalogsAsync(
        [AsParameters] ListCatalogsQueryRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancelattionToken
    ) => await mediator.Send(request, cancelattionToken);

    private static async Task<CreateCatalogCommandResponse> CreateCatalogAsync(
        [FromBody] CreateCatalogCommandRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(request, cancellationToken);

    private static async Task<UpdateCatalogCommandResponse> UpdateCatalogAsync(
        [FromRoute] Guid key,
        [FromBody] UpdateCatalogCommandRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        request.Key = key;
        return await mediator.Send(request, cancellationToken);
    }

    private static async Task DeleteCatalogAsync(
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(new DeleteCatalogCommandRequest { Key = key }, cancellationToken);
}
