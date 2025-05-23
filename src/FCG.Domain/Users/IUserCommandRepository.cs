
namespace FCG.Domain.Users;
public interface IUserCommandRepository : IUserRepository
{
    Task<bool> ExistByEmailAsync(
        string email,
        Guid? ignoreKey = null,
        CancellationToken cancellationToken = default
    );

    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
