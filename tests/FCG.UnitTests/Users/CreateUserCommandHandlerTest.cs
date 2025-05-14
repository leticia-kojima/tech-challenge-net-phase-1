using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Commands;
using FCG.Domain._Common.Exceptions;
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
    public async Task ShouldCreateUserAsync()
    {
        var request = AutoFaker.Generate<CreateUserCommandRequest>();

        request.FullName = "Colt Macias";
        request.Email = "colt@email.com";
        request.Role = ERole.User;
        request.Password = "password_example";

        var command = new CreateUserCommandHandler(_repository, _mediator);

        var result = await command.Handle(request, _cancellationToken);

        Assert.NotEqual(Guid.Empty, result.Key);
        Assert.Equal(request.FullName, result.FullName);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.Role, result.Role);
        await _repository
            .Received(1)
            .AddAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
    }

    [Theory]
    [InlineData(null, "colt@email.com", ERole.User, "password_example", "FullName is required.")]
    [InlineData("", "colt@email.com", ERole.User, "password_example", "FullName is required.")]
    [InlineData(" ", "colt@email.com", ERole.User, "password_example", "FullName is required.")]
    [InlineData("Colt Macias", null, ERole.User, "password_example", "Email is required.")]
    [InlineData("Colt Macias", "", ERole.User, "password_example", "Email is required.")]
    [InlineData("Colt Macias", " ", ERole.User, "password_example", "Email is required.")]
    [InlineData("Colt Macias", "colt@email.com", ERole.User, null, "Password is required.")]
    [InlineData("Colt Macias", "colt@email.com", ERole.User, "", "Password is required.")]
    [InlineData("Colt Macias", "colt@email.com", ERole.User, " ", "Password is required.")]
    public async Task ShouldThrowValidationExceptionAsync(
        string? fullName,
        string? email,
        ERole role,
        string? password,
        string expectedMessage
    )
    {
        var request = new CreateUserCommandRequest
        {
            FullName = fullName,
            Email = email,
            Role = role,
            Password = password
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
