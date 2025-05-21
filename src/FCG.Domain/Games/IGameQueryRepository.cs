namespace FCG.Domain.Games;
public interface IGameQueryRepository : IGameRepository
{
    Task<Game?> GetByKeyAsync(
        Guid key,
        CancellationToken? cancellationToken = null
    );
    
    Task<IEnumerable<Game>> GetAllAsync(
        CancellationToken? cancellationToken = null
    );
    
    Task<IEnumerable<Game>> GetByCatalogKeyAsync(
        Guid catalogKey,
        CancellationToken? cancellationToken = null
    );
}

