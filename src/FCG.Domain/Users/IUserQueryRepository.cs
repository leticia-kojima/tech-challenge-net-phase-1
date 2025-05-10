namespace FCG.Domain.Users;
public interface IUserQueryRepository : IUserRepository
{
    Task<IReadOnlyCollection<User>> SearchAsync(string? search, CancellationToken cancellationToken);
}
