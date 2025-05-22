using FCG.Application.Contracts.Games.Commands;
using FCG.Application.Contracts.Games.Queries;
using FCG.Domain.Catalogs;
using FCG.Domain.Games;
using FCG.Domain._Common.Exceptions;
using FCG.Infrastructure.Contexts.FCGCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
    )
    {
        try
        {
            var response = await mediator.Send(new GetGameByKeyQueryRequest { Key = key }, cancellationToken);
            if (response == null)
                throw new FCGNotFoundException(key, nameof(Game), $"Game with key {key} not found");
                
            return response;
        }
        catch (Exception ex) when (ex is not FCGNotFoundException)
        {
            // Registre o erro em log aqui se necessário
            throw new Exception($"Error retrieving game with key {key}: {ex.Message}", ex);
        }
    }

    private static async Task<IEnumerable<GetAllGamesQueryResponse>> GetAllGamesAsync(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) => await mediator.Send(new GetAllGamesQueryRequest(), cancellationToken);

    private static async Task<IEnumerable<GetGamesByCatalogQueryResponse>> GetGamesByCatalogAsync(
        [FromRoute] Guid catalogKey,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    ) 
    {
        try
        {
            return await mediator.Send(new GetGamesByCatalogQueryRequest { CatalogKey = catalogKey }, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving games for catalog {catalogKey}: {ex.Message}", ex);
        }
    }

    private static async Task<CreateGameCommandResponse> CreateGameAsync(
        [FromBody] CreateGameCommandRequest request,
        [FromServices] IMediator mediator,
        [FromServices] FCGCommandsDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        // Validações básicas
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Request cannot be null");
            
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new FCGValidationException(nameof(request.Title), "Title is required");
            
        if (string.IsNullOrWhiteSpace(request.Description))
            throw new FCGValidationException(nameof(request.Description), "Description is required");
            
        if (request.Title.Length > 100)
            throw new FCGValidationException(nameof(request.Title), "Title cannot exceed 100 characters");
            
        if (request.Description.Length > 500)
            throw new FCGValidationException(nameof(request.Description), "Description cannot exceed 500 characters");

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
        else
        {
            // Verificar se o catálogo especificado existe
            var catalogExists = await dbContext.Set<Catalog>()
                .AnyAsync(c => c.Key == request.CatalogKey, cancellationToken);
                
            if (!catalogExists)
                throw new FCGNotFoundException(request.CatalogKey, nameof(Catalog), $"Catalog with key {request.CatalogKey} not found");
        }
        
        try
        {
            return await mediator.Send(request, cancellationToken);
        }
        catch (FCGDuplicateException)
        {
            throw; // Deixa a exceção de duplicação passar normalmente
        }
        catch (Exception ex)
        {
            // Registre o erro em log aqui se necessário
            throw new Exception($"Error creating game: {ex.Message}", ex);
        }
    }

    private static async Task<UpdateGameCommandResponse> UpdateGameAsync(
        [FromRoute] Guid key,
        [FromBody] UpdateGameCommandRequest request,
        [FromServices] IMediator mediator,
        [FromServices] FCGCommandsDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        // Validações básicas
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Request cannot be null");
            
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new FCGValidationException(nameof(request.Title), "Title is required");
            
        if (string.IsNullOrWhiteSpace(request.Description))
            throw new FCGValidationException(nameof(request.Description), "Description is required");
            
        if (request.Title.Length > 100)
            throw new FCGValidationException(nameof(request.Title), "Title cannot exceed 100 characters");
            
        if (request.Description.Length > 500)
            throw new FCGValidationException(nameof(request.Description), "Description cannot exceed 500 characters");

        // Verificar se o jogo existe antes de tentar atualizar
        var gameExists = await dbContext.Set<Game>().AnyAsync(g => g.Key == key, cancellationToken);
        if (!gameExists)
            throw new FCGNotFoundException(key, nameof(Game), $"Game with key {key} not found");

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
        else
        {
            // Verificar se o catálogo especificado existe
            var catalogExists = await dbContext.Set<Catalog>()
                .AnyAsync(c => c.Key == request.CatalogKey, cancellationToken);
                
            if (!catalogExists)
                throw new FCGNotFoundException(request.CatalogKey, nameof(Catalog), $"Catalog with key {request.CatalogKey} not found");
        }
        
        try
        {
            return await mediator.Send(request, cancellationToken);
        }
        catch (FCGDuplicateException)
        {
            throw; // Deixa a exceção de duplicação passar normalmente
        }
        catch (Exception ex)
        {
            // Registre o erro em log aqui se necessário
            throw new Exception($"Error updating game with key {key}: {ex.Message}", ex);
        }
    }

    private static async Task DeleteGameAsync(
        [FromRoute] Guid key,
        [FromServices] IMediator mediator,
        [FromServices] FCGCommandsDbContext dbContext,
        CancellationToken cancellationToken
    ) 
    {
        // Verificar se o jogo existe antes de tentar excluir
        var gameExists = await dbContext.Set<Game>().AnyAsync(g => g.Key == key, cancellationToken);
        if (!gameExists)
            throw new FCGNotFoundException(key, nameof(Game), $"Game with key {key} not found");
            
        try
        {
            await mediator.Send(new DeleteGameCommandRequest { Key = key }, cancellationToken);
        }
        catch (Exception ex)
        {
            // Registre o erro em log aqui se necessário
            throw new Exception($"Error deleting game with key {key}: {ex.Message}", ex);
        }
    }

    private static async Task<IResult> CategorizeGameAsync(
        [FromRoute] Guid key,
        [FromBody] CategorizeGameRequest request,
        [FromServices] IMediator mediator,
        [FromServices] FCGCommandsDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        if (request == null)
            return Results.BadRequest("Request cannot be null");
            
        if (request.CatalogKey == Guid.Empty)
            return Results.BadRequest("Catalog key is required");
            
        // Verificar se o jogo existe
        var game = await dbContext.Set<Game>().FirstOrDefaultAsync(g => g.Key == key, cancellationToken);
        if (game == null)
            return Results.NotFound($"Game with key {key} not found");
            
        // Verificar se o catálogo existe
        var catalog = await dbContext.Set<Catalog>().FirstOrDefaultAsync(c => c.Key == request.CatalogKey, cancellationToken);
        if (catalog == null)
            return Results.BadRequest($"Catalog with key {request.CatalogKey} not found");
            
        try
        {
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
        catch (Exception ex)
        {
            return Results.Problem($"Error categorizing game: {ex.Message}");
        }
    }
}
