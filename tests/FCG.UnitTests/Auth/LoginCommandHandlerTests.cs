using FCG.Application.Commands.Auth;
using FCG.Domain.Users;
using System.IdentityModel.Tokens.Jwt;

namespace FCG.UnitTests.Auth;
public class LoginCommandHandlerTests : TestHandlerBase<LoginCommandHandler>
{
    private readonly IUserCommandRepository _userCommandRepository;

    public LoginCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _userCommandRepository = GetMock<IUserCommandRepository>();
    }

    [Fact]
    public async Task ShouldAthenticateAsync()
    {
        var request = _modelBuilder.LoginCommandRequest.Generate();
        var user = _entityBuilder.User
            .RuleFor(u => u.Email, new Email(request.Email))
            .RuleFor(u => u.PasswordHash, new Password(request.Password))
            .Generate();

        _userCommandRepository
            .GetByEmailAsync(user.Email, _cancellationToken)
            .Returns(request.Email == user.Email ? user : null);

        var result = await Handler.Handle(request, _cancellationToken);
        
        result.ShouldNotBeNull();
        result.Token.ShouldNotBeNullOrEmpty();

        var utcNow = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(result.Token);

        utcNow.ShouldBeGreaterThan(jwt.ValidFrom);
        utcNow.ShouldBeLessThan(jwt.ValidTo);
        utcNow.ShouldBeLessThan(result.Expiration);
        jwt.Claims.ShouldNotBeEmpty();
    }

    [Theory]
    [InlineData("wrongemail@fcg.test.com", "3SfE@43NR1#b")]
    [InlineData("email@fcg.test.com", "wrongpassword")]
    public async Task ShouldThrowExceptionForInvalidCredentialsAsync(
        string email,
        string password
    )
    {
        var request = _modelBuilder.LoginCommandRequest
            .RuleFor(r => r.Email, email)
            .RuleFor(r => r.Password, password)
            .Generate();
        var user = _entityBuilder.User
            .RuleFor(u => u.Email, new Email("email@fcg.test.com"))
            .RuleFor(u => u.PasswordHash, new Password("3SfE@43NR1#b"))
            .Generate();

        _userCommandRepository
            .GetByEmailAsync(user.Email, _cancellationToken)
            .Returns(request.Email == user.Email ? user : null);

        var validationException = await Should.ThrowAsync<FCGValidationException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        validationException.Message
            .ShouldBe($"Invalid credentials for user with email '{request.Email}'.");
    }
}