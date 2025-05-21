using FCG.Application.Contracts.Games.Commands;
using FCG.Application.Contracts.Games.Queries;
using FCG.Domain.Catalogs;
using FCG.Domain.Games;
using FCG.Infrastructure.Contexts.FCGCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FCG.API.Endpoints;

public static class GamesEndpoints
{
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var gamesGroup = app.MapGroup("/games");

        gamesGroup.MapGet("/", GetAllGamesAsync);
        gamesGroup.MapGet("/{key:guid}", GetGameByKeyAsync);
        gamesGroup.MapGet("/catalog/{catalogKey:guid}", GetGamesByCatalogAsync);
        gamesGroup.MapPost("/", CreateGameAsync);
        gamesGroup.MapPut("/{key:guid}", UpdateGameAsync);
        gamesGroup.MapDelete("/{key:guid}", DeleteGameAsync);
        
        // Novo endpoint para categorizar jogos
        gamesGroup.MapPut("/{key:guid}/categorize", CategorizeGameAsync);
    }

    private static async Task<GetGameByKeyQueryResponse> GetGameByKeyAsync(
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(new GetGameByKeyQueryRequest { Key = key }, cancellationToken);

    private static async Task<IEnumerable<GetAllGamesQueryResponse>> GetAllGamesAsync(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(new GetAllGamesQueryRequest(), cancellationToken);

    private static async Task<IEnumerable<GetGamesByCatalogQueryResponse>> GetGamesByCatalogAsync(
        [FromRoute] Guid catalogKey,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(new GetGamesByCatalogQueryRequest { CatalogKey = catalogKey }, cancellationToken);

    private static async Task<CreateGameCommandResponse> CreateGameAsync(
        [FromBody] CreateGameCommandRequest request,
        [FromServices] IMediator mediator,
        [FromServices] FCGCommandsDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        // Se não foi fornecido um CatalogKey, vamos buscar ou criar um catálogo padrão
        if (request.CatalogKey == Guid.Empty)
        {
            var defaultCatalog = await dbContext.Set<Catalog>().FirstOrDefaultAsync(cancellationToken);
            
            if (defaultCatalog == null)
            {
                defaultCatalog = new Catalog(
                    Guid.NewGuid(),
                    "Default Games Category",
                    "Default category for all games"
                );
                
                await dbContext.Set<Catalog>().AddAsync(defaultCatalog, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            
            request.CatalogKey = defaultCatalog.Key;
        }
        
        return await mediator.Send(request, cancellationToken);
    }

    private static async Task<UpdateGameCommandResponse> UpdateGameAsync(
        [FromRoute] Guid key,
        [FromBody] UpdateGameCommandRequest request,
        [FromServices] IMediator mediator,
        [FromServices] FCGCommandsDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        request.Key = key;
        
        // Se não foi fornecido um CatalogKey, vamos buscar ou criar um catálogo padrão
        if (request.CatalogKey == Guid.Empty)
        {
            var defaultCatalog = await dbContext.Set<Catalog>().FirstOrDefaultAsync(cancellationToken);
            
            if (defaultCatalog == null)
            {
                defaultCatalog = new Catalog(
                    Guid.NewGuid(),
                    "Default Games Category",
                    "Default category for all games"
                );
                
                await dbContext.Set<Catalog>().AddAsync(defaultCatalog, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            
            request.CatalogKey = defaultCatalog.Key;
        }
        
        return await mediator.Send(request, cancellationToken);
    }

    private static async Task DeleteGameAsync(
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(new DeleteGameCommandRequest { Key = key }, cancellationToken);

    private static async Task<IResult> CategorizeGameAsync(
        [FromRoute] Guid key,
        [FromBody] CategorizeGameRequest request,
        [FromServices] IMediator mediator,
        [FromServices] FCGCommandsDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var game = await dbContext.Set<Game>().FirstOrDefaultAsync(g => g.Key == key, cancellationToken);
        if (game == null)
            return Results.NotFound($"Game with id {key} not found");
            
        // Verificar se o catálogo existe
        var catalog = await dbContext.Set<Catalog>().FirstOrDefaultAsync(c => c.Key == request.CatalogKey, cancellationToken);
        if (catalog == null)
            return Results.BadRequest($"Catalog with id {request.CatalogKey} not found");
            
        // Atualizar a categoria do jogo
        var updateRequest = new UpdateGameCommandRequest
        {
            Key = key,
            Title = game.Title,
            Description = game.Description,
            CatalogKey = request.CatalogKey
        };
        
        await mediator.Send(updateRequest, cancellationToken);
        
        return Results.Ok($"Game successfully categorized to: {catalog.Name}");
    }
}
