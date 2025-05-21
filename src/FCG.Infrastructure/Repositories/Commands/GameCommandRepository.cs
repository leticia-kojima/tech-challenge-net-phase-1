using FCG.Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories.Commands;
public class GameCommandRepository : FCGCommandRepositoryBase<Game>, IGameCommandRepository
{
    public GameCommandRepository(FCGCommandsDbContext context) : base(context)
    {
    }

    public Task<bool> ExistByTitleAsync(string title, Guid? ignoreKey = null, CancellationToken? cancellationToken = null)
        => _dbSet.AnyAsync(g => g.Title == title && g.Key != ignoreKey, cancellationToken ?? CancellationToken.None);
        
    public void Detach(Game entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
    }
}
