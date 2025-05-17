using FCG.Application.Contracts.Users.Queries;
using FCG.Domain.Users;

namespace FCG.Application.Queries.Users;
public class GetUserQueryHandler : IQueryHandler<GetUserQueryRequest, UserQueryResponse>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public GetUserQueryHandler(IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }

    public async Task<UserQueryResponse> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _userQueryRepository.GetByIdAsync(request.Key, cancellationToken);

        if (user is null) throw new FCGNotFoundException(request.Key, nameof(User), $"User with key '{request.Key}' was not found.");

        return new UserQueryResponse(user);
    }
}
