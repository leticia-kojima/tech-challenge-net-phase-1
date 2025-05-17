namespace FCG.Application.Contracts.Users.Queries;
public class ListUsersQueryRequest : IQuery<IReadOnlyCollection<UserQueryResponse>>
{
    public string? Search { get; set; }
}
