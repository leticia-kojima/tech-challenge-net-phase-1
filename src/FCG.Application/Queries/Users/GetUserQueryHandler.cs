using FCG.Application.Contracts.Users.Queries;
using FCG.Domain._Common.Exceptions;
using FCG.Domain.Users;

namespace FCG.Application.Queries.Users;
public class GetUserQueryHandler : IQueryHandler<GetUserQueryRequest, UserQueryResponse>
{
    private readonly IUserQueryRepository _repository;

    public GetUserQueryHandler(IUserQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserQueryResponse> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Key, cancellationToken);

        if (user is null) throw new FCGNotFoundException(request.Key, nameof(User), "User not found.");

        return new UserQueryResponse(user);
    }
}
