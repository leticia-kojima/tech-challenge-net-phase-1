using FCG.Domain._Common;

namespace FCG.Domain.Users;
public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
