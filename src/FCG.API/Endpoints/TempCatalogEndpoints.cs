using FCG.Domain.Catalogs;
using FCG.Infrastructure.Contexts.FCGCommands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FCG.API.Endpoints;

public static class TempCatalogEndpoints
{
    public static void MapTempCatalogEndpoints(this WebApplication app)
    {
        app.MapGet("/temp/catalogs", async (
            [FromServices] FCGCommandsDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var catalogs = await dbContext.Set<Catalog>().ToListAsync(cancellationToken);
            return Results.Ok(catalogs);
        });

        app.MapPost("/temp/catalogs", async (
            [FromServices] FCGCommandsDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var catalog = new Catalog(
                Guid.NewGuid(),
                "Games",
                "Video games category"
            );
            
            await dbContext.Set<Catalog>().AddAsync(catalog, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Results.Ok(catalog);
        });

        // Endpoint para criar um catálogo padrão se não existir e retorná-lo
        app.MapGet("/temp/catalogs/default", async (
            [FromServices] FCGCommandsDbContext dbContext,
            CancellationToken cancellationToken) =>
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
            
            return Results.Ok(defaultCatalog);
        });
    }
}
