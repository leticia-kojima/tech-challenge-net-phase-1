using FCG.Application.Contracts.Users.Queries;

namespace FCG.Application.Queries.Users;
public class GetUserQueryHandler : IQueryHandler<GetUserQueryRequest, UserQueryResponse>
{
    public Task<UserQueryResponse> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
