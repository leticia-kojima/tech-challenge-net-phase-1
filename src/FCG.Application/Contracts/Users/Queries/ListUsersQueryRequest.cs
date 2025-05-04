using MediatR;

namespace FCG.Application.Contracts.Users.Queries;
public class ListUsersQueryRequest : IRequest<IEnumerable<UserQueryResponse>>
{
    public string? Search { get; set; }
}
