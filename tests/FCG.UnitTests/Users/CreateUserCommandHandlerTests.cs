using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class CreateUserCommandHandlerTests : TestHandlerBase<CreateUserCommandHandler>
{
    private readonly IMediator _mediator;
    private readonly IUserCommandRepository _userCommandRepository;

    public CreateUserCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _mediator = GetMock<IMediator>();
        _userCommandRepository = GetMock<IUserCommandRepository>();
    }

    [Fact]
    public async Task ShouldCreateUserAsync()
    {
        var request = _modelBuilder.CreateUserCommandRequest.Generate();

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Key.ShouldNotBe(Guid.Empty);
        result.FullName.ShouldBe(request.FullName);
        result.Email.ShouldBe(request.Email);
        await _userCommandRepository
            .Received(1)
            .AddAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
        await _mediator
            .Received(1)
            .Publish(
                Arg.Any<UserCreatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowExcpetionForUserDuplicationAsync()
    {
        var request = _modelBuilder.CreateUserCommandRequest.Generate();

        _userCommandRepository.ExistByEmailAsync(request.Email, cancellationToken: _cancellationToken)
            .Returns(true);

        var duplicateException = await Should.ThrowAsync<FCGDuplicateException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        duplicateException.Message
            .ShouldBe($"The email '{request.Email}' is already in use.");
        await _userCommandRepository
            .DidNotReceive()
            .AddAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<UserCreatedEvent>(),
                _cancellationToken
            );
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
            .RuleFor(u => u.Password, password)
            .Generate();

        var validationException = await Should.ThrowAsync<FCGValidationException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        validationException.Message
            .ShouldBe(expectedMessage);
        await _userCommandRepository
            .DidNotReceive()
            .AddAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<UserCreatedEvent>(),
                _cancellationToken
            );
    }
}
