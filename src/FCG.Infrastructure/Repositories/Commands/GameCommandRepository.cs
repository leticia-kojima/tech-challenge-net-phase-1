using FCG.Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories.Commands;
public class GameCommandRepository : FCGCommandRepositoryBase<Game>, IGameCommandRepository
{
    public GameCommandRepository(FCGCommandsDbContext context) : base(context)
    {
    }

    public Task<bool> ExistByTitleAsync(string title, Guid? ignoreKey = null, CancellationToken cancellationToken = default)
        => _dbSet.AnyAsync(g => g.Title == title && g.Key != ignoreKey, cancellationToken);
}
