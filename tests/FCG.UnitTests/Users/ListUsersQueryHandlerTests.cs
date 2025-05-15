using FCG.Application.Contracts.Users.Queries;
using FCG.Application.Queries.Users;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class ListUsersQueryHandlerTests : TestHandlerBase<ListUsersQueryHandler>
{
    private readonly IUserQueryRepository _repository;

    public ListUsersQueryHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _repository = GetMock<IUserQueryRepository>();
    }

    [Fact]
    public async Task ShouldGetListAsync()
    {
        var userName = _faker.Internet.UserName();
        var request = new ListUsersQueryRequest { Search = userName };
        var users = _entityBuilder.User
            .RuleFor(u => u.FullName, userName)
            .Generate(5);
        
        _repository.SearchAsync(request.Search, _cancellationToken)
            .Returns(users);

        var result = await Handler.Handle(request, _cancellationToken);
        
        result.ShouldNotBeNull();
        result.Count.ShouldBe(users.Count);
    }
}
