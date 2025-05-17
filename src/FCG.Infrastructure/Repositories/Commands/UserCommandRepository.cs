using FCG.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories.Commands;
public class UserCommandRepository : FCGCommandRepositoryBase<User>, IUserCommandRepository
{
    public UserCommandRepository(FCGCommandsDbContext context) : base(context)
    {

    }

    public Task<bool> ExistByEmailAsync(string email, Guid? ignoreKey = null, CancellationToken? cancellationToken = null)
        => _dbSet.AnyAsync(u => u.Email == email && u.Key != ignoreKey);
}
