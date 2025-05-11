using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Commands;
using FCG.Domain._Common;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class CreateUserCommandHandlerTest : TestBase
{
    private readonly IUserCommandRepository _repository;
    private readonly IMediator _mediator;

    public CreateUserCommandHandlerTest(FCGFixture fixture) : base(fixture)
    {
        _repository = Substitute.For<IUserCommandRepository>();
        _mediator = Substitute.For<IMediator>();
    }

    [Fact]
    // This is a demo unit test!
    public async Task ShouldCreateUserAsync()
    {
        var request = AutoFaker.Generate<CreateUserCommandRequest>();

        request.FullName = "Colt Macias";
        request.Email = "colt@email.com";
        request.Role = UserRole.User;
        request.PasswordHash = "hashed_password_example";

        var command = new CreateUserCommandHandler(_repository, _mediator);

        var result = await command.Handle(request, _cancellationToken);

        Assert.NotEqual(Guid.Empty, result.Key);
        Assert.Equal(request.FullName, result.FullName);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.Role, result.Role);
        Assert.Equal(request.PasswordHash, result.PasswordHash);
        await _repository
            .Received(1)
            .AddAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
    }

    [Theory]
    [Theory]
    [InlineData(null, "colt@email.com", UserRole.User, "hashed_password_example", "FullName is required.")]
    [InlineData("", "colt@email.com", UserRole.User, "hashed_password_example", "FullName is required.")]
    [InlineData(" ", "colt@email.com", UserRole.User, "hashed_password_example", "FullName is required.")]
    [InlineData("Colt Macias", null, UserRole.User, "hashed_password_example", "Email is required.")]
    [InlineData("Colt Macias", "", UserRole.User, "hashed_password_example", "Email is required.")]
    [InlineData("Colt Macias", " ", UserRole.User, "hashed_password_example", "Email is required.")]
    [InlineData("Colt Macias", "colt@email.com", UserRole.User, null, "PasswordHash is required.")]
    [InlineData("Colt Macias", "colt@email.com", UserRole.User, "", "PasswordHash is required.")]
    [InlineData("Colt Macias", "colt@email.com", UserRole.User, " ", "PasswordHash is required.")]
    // This is a demo unit test!
    public async Task ShouldThrowValidationExceptionAsync(
        string? fullName,
        string? email,
        UserRole role,
        string? passwordHash,
        string expectedMessage
    )
    {
        var request = new CreateUserCommandRequest
        {
            FullName = fullName,
            Email = email,
            Role = role,
            PasswordHash = passwordHash
        };
        var command = new CreateUserCommandHandler(_repository, _mediator);

        var validationException = await Assert.ThrowsAsync<FCGValidationException>(
            () => command.Handle(request, _cancellationToken)
        );

        Assert.NotNull(validationException);
        Assert.Equal(expectedMessage, validationException.Message);
        await _repository
            .DidNotReceive()
            .AddAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
    }
}
