using FCG.Application._Common.Contracts;

namespace FCG.Application.Contracts.Users.Queries;
public class ListUsersQueryRequest : IQueryList<UserQueryResponse>
{
    public string? Search { get; set; }
}
