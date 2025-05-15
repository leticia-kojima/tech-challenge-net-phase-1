using FCG.Application.Contracts.Users.Queries;
using FCG.Application.Queries.Users;
using FCG.Domain._Common.Exceptions;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class GetUserQueryHandlerTests : TestHandlerBase<GetUserQueryHandler>
{
    private readonly IUserQueryRepository _repository;

    public GetUserQueryHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _repository = GetMock<IUserQueryRepository>();
    }

    [Fact]
    public async Task ShouldGetAsync()
    {
        var user = _entityBuilder.User.Generate();
        var request = new GetUserQueryRequest { Key = user.Key };

        _repository.GetByIdAsync(user.Key, _cancellationToken)
            .Returns(user);

        var result = await Handler.Handle(request, _cancellationToken);

        result.Key.ShouldBe(user.Key);
        result.FullName.ShouldBe(user.FullName);
        result.Email.ShouldBe(user.Email);
        result.Role.ShouldBe(user.Role);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = new GetUserQueryRequest { Key = Guid.NewGuid() };
        _repository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as User);

        var exception = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        exception.Message.ShouldBe("User not found.");
    }
}
