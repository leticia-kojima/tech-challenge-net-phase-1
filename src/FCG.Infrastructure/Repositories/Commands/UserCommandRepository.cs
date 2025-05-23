using FCG.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories.Commands;
public class UserCommandRepository : FCGCommandRepositoryBase<User>, IUserCommandRepository
{
    public UserCommandRepository(FCGCommandsDbContext context) : base(context)
    {

    }

    public Task<bool> ExistByEmailAsync(string email, Guid? ignoreKey = null, CancellationToken cancellationToken = default)
        => _dbSet.AnyAsync(u => u.Email == email && u.Key != ignoreKey, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
}
