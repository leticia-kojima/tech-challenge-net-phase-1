using FCG.Domain.Users;

namespace FCG.Infrastructure.Repositories.Commands;
public class UserCommandRepository : FCGCommandRepositoryBase<User>, IUserCommandRepository
{
    public UserCommandRepository(FCGCommandsDbContext context) : base(context)
    {

    }
}
