using FCG.Application.Contracts.Users.Queries;
using FCG.Application.Queries.Users;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class GetUserQueryHandlerTests : TestHandlerBase<GetUserQueryHandler>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public GetUserQueryHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _userQueryRepository = GetMock<IUserQueryRepository>();
    }

    [Fact]
    public async Task ShouldGetAsync()
    {
        var user = _entityBuilder.User.Generate();
        var request = new GetUserQueryRequest { Key = user.Key };

        _userQueryRepository.GetByIdAsync(user.Key, _cancellationToken)
            .Returns(user);

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Key.ShouldBe(user.Key);
        result.FullName.ShouldBe(user.FullName);
        result.Email.ShouldBe(user.Email);
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = new GetUserQueryRequest { Key = Guid.NewGuid() };
        _userQueryRepository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as User);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        notFoundException.Message.ShouldBe($"User with key '{request.Key}' was not found.");
    }
}
