namespace FCG.Domain._Common;
public interface IRepository;

public interface IRepository<TEntity> : IRepository
    where TEntity : EntityBase
{
    Task<TEntity?> GetByIdAsync(Guid key, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    Task DeleteAsync(Guid key, CancellationToken cancellationToken);
}
