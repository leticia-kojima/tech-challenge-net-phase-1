using FCG.Application.Contracts.Users.Queries;
using FCG.Domain.Users;

namespace FCG.Application.Queries.Users;
public class ListUsersQueryHandler : IQueryHandler<ListUsersQueryRequest, IReadOnlyCollection<UserQueryResponse>>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public ListUsersQueryHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    public async Task<IReadOnlyCollection<UserQueryResponse>> Handle(ListUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var users = await _userQueryRepository.SearchAsync(request.Search, cancellationToken);

        return users.Select(u => new UserQueryResponse(u)).ToArray();
    }
}
