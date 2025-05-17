namespace FCG.Application.Contracts.Users.Queries;
public class GetUserQueryRequest : IQuery<UserQueryResponse>
{
    public Guid Key { get; set; }
}
