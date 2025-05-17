using FCG.Application.Commands.Auth;
using FCG.Domain._Common.Settings;
using FCG.Domain.Users;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace FCG.UnitTests.Auth;
public class LoginCommandHandlerTests : TestBase
{
    private readonly LoginCommandHandler _handler;
    private readonly IUserCommandRepository _userCommandRepository;

    public LoginCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "yHTE1H5d27zH0nxDGBE7quDPg2BRuYkJ8efWrKCMZfQ=",
            Issuer = "FCG",
            Audience = "FCG",
            ExpirationInHours = 24
        };

        _userCommandRepository = Substitute.For<IUserCommandRepository>();
        _handler = new LoginCommandHandler(
            Options.Create(jwtSettings),
            _userCommandRepository
        );
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

        var result = await _handler.Handle(request, _cancellationToken);
        
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
    [InlineData(null, "somepassword", "Email is required.")]
    [InlineData("", "somepassword", "Email is required.")]
    [InlineData("user@fcg.test.com", null, "Password is required.")]
    [InlineData("user@fcg.test.com", "", "Password is required.")]
    public async Task ShouldThrowExceptionForMissingEmailOrPasswordAsync(
        string? email,
        string? password,
        string expectedMessage
    )
    {
        var request = _modelBuilder.LoginCommandRequest
            .RuleFor(r => r.Email, email)
            .RuleFor(r => r.Password, password)
            .Generate();

        var validationException = await Should.ThrowAsync<FCGValidationException>(
            () => _handler.Handle(request, _cancellationToken)
        );

        validationException.Message
            .ShouldBe(expectedMessage);
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
            () => _handler.Handle(request, _cancellationToken)
        );

        validationException.Message
            .ShouldBe($"Invalid credentials for user with email '{request.Email}'.");
    }
}