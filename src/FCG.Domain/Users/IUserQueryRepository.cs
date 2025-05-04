
namespace FCG.Domain.Users;
public interface IUserQueryRepository : IUserRepository
{
    Task<IEnumerable<User>> SearchAsync(string? search, CancellationToken cancellationToken);
}
