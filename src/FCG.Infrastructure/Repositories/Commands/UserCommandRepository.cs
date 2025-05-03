using FCG.Domain.Users;
using FCG.Infrastructure._Common.Persistence;
using FCG.Infrastructure.Contexts.FCGCommands;

namespace FCG.Infrastructure.Repositories.Commands;
public class UserCommandRepository : FCGCommandRepositoryBase<User>, IUserCommandRepository
{
    public UserCommandRepository(FCGCommandsDbContext context) : base(context)
    {
    }
}
