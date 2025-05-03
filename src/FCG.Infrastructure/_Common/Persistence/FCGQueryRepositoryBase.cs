using FCG.Domain._Common;
using FCG.Infrastructure.Contexts.FCGQueries;
using Microsoft.EntityFrameworkCore;
using MongoFramework;

namespace FCG.Infrastructure._Common.Persistence;
public class FCGQueryRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
{
    protected readonly FCGQueriesDbContext _context;
    protected readonly IMongoDbSet<TEntity> _dbSet;

    public FCGQueryRepositoryBase(FCGQueriesDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        _dbSet.RemoveById(id);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToArrayAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Key == id, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}