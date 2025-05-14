using FCG.Application.Commands.Users;
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
        var request = _modelBuilder.CreateUserCommandRequest.Generate();
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

    [Fact]
    public async Task ShouldThrowExcpetionForUserDuplicationAsync()
    {
        var request = _modelBuilder.CreateUserCommandRequest.Generate();

        _repository.ExistByEmailAsync(request.Email, cancellationToken: _cancellationToken)
            .Returns(true);

        var command = new CreateUserCommandHandler(_repository, _mediator);
        var duplicateException = await Assert.ThrowsAsync<FCGDuplicateException>(
            () => command.Handle(request, _cancellationToken)
        );

        Assert.NotNull(duplicateException);
        Assert.Equal("An user with this email already exists.", duplicateException.Message);
    }

    [Theory]
    [InlineData(null, "colt@email.com", ERole.User, "3rNDl7}<Y3^4", "FullName is required.")]
    [InlineData("", "colt@email.com", ERole.User, "3rNDl7}<Y3^4", "FullName is required.")]
    [InlineData(" ", "colt@email.com", ERole.User, "3rNDl7}<Y3^4", "FullName is required.")]
    [InlineData("Colt Macias", null, ERole.User, "3rNDl7}<Y3^4", "Email is required.")]
    [InlineData("Colt Macias", "", ERole.User, "3rNDl7}<Y3^4", "Email is required.")]
    [InlineData("Colt Macias", " ", ERole.User, "3rNDl7}<Y3^4", "Email is required.")]
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
        var request = _modelBuilder.CreateUserCommandRequest
            .RuleFor(u => u.FullName, fullName)
            .RuleFor(u => u.Email, email)
            .RuleFor(u => u.Role, role)
            .RuleFor(u => u.Password, password)
            .Generate();
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
