using FCG.Domain._Common;
using FCG.Infrastructure.Contexts.FCGCommands;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure._Common.Persistence;
public abstract class FCGCommandsRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
{
    protected readonly FCGCommandsDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected FCGCommandsRepositoryBase(FCGCommandsDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToArrayAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
