using FCG.Domain.Games;
using FCG.Infrastructure.Contexts.FCGCommands;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories.Queries;
public class GameQueryRepository : IGameQueryRepository
{
    private readonly FCGCommandsDbContext _context;

    public GameQueryRepository(FCGCommandsDbContext context)
    {
        _context = context;
    }

    public async Task<Game?> GetByKeyAsync(Guid key, CancellationToken? cancellationToken = null)
    {
        return await _context.Set<Game>()
            .Include(g => g.Catalog)
            .FirstOrDefaultAsync(g => g.Key == key, cancellationToken ?? CancellationToken.None);
    }
    
    public async Task<IEnumerable<Game>> GetAllAsync(CancellationToken? cancellationToken = null)
    {
        return await _context.Set<Game>()
            .Include(g => g.Catalog)
            .Where(g => g.DeletedAt == null)
            .ToListAsync(cancellationToken ?? CancellationToken.None);
    }
    
    public async Task<IEnumerable<Game>> GetByCatalogKeyAsync(Guid catalogKey, CancellationToken? cancellationToken = null)
    {
        return await _context.Set<Game>()
            .Include(g => g.Catalog)
            .Where(g => g.CatalogKey == catalogKey && g.DeletedAt == null)
            .ToListAsync(cancellationToken ?? CancellationToken.None);
    }

    // Implementação dos métodos obrigatórios de IRepository<Game>
    
    public async Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<Game>()
            .Include(g => g.Catalog)
            .FirstOrDefaultAsync(g => g.Key == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Game>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<Game>()
            .Include(g => g.Catalog)
            .Where(g => g.DeletedAt == null)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Game entity, CancellationToken cancellationToken)
    {
        await _context.Set<Game>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Game entity, CancellationToken cancellationToken)
    {
        _context.Set<Game>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Game entity, CancellationToken cancellationToken)
    {
        // Exclusão lógica (soft delete)
        entity.Delete();  // Usando o método correto Delete() em vez de MarkAsDeleted()
        _context.Set<Game>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            await DeleteAsync(entity, cancellationToken);
        }
    }
}
