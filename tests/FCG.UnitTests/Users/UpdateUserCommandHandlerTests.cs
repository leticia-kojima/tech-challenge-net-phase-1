using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Events;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class UpdateUserCommandHandlerTests : TestHandlerBase<UpdateUserCommandHandler>
{
    private readonly IMediator _mediator;
    private readonly IUserCommandRepository _userCommandRepository;

    public UpdateUserCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _mediator = GetMock<IMediator>();
        _userCommandRepository = GetMock<IUserCommandRepository>();
    }

    [Fact]
    public async Task ShouldUpdateUserAsync()
    {
        var user = _entityBuilder.User.Generate();
        var request = _modelBuilder.UpdateUserCommandRequest
            .RuleFor(u => u.Key, user.Key)
            .Generate();

        _userCommandRepository.GetByIdAsync(user.Key, _cancellationToken).Returns(user);

        var result = await Handler.Handle(request, _cancellationToken);

        result.ShouldNotBeNull();
        result.Key.ShouldNotBe(Guid.Empty);
        result.FullName.ShouldBe(request.FullName);
        result.Email.ShouldBe(request.Email);
        await _userCommandRepository
            .Received(1)
            .UpdateAsync(
                Arg.Is<User>(u => u.Key == request.Key
                    && u.FullName == request.FullName
                    && u.Email == request.Email),
                _cancellationToken
            );
        await _mediator
            .Received(1)
            .Publish(
                Arg.Any<UserUpdatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionAsync()
    {
        var request = _modelBuilder.UpdateUserCommandRequest
            .Generate();

        _userCommandRepository.GetByIdAsync(request.Key, _cancellationToken)
            .Returns(null as User);

        var notFoundException = await Should.ThrowAsync<FCGNotFoundException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        notFoundException.Message
            .ShouldBe($"User with key '{request.Key}' was not found.");
        await _userCommandRepository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<UserUpdatedEvent>(),
                _cancellationToken
            );
    }

    [Fact]
    public async Task ShouldThrowExcpetionForUserDuplicationAsync()
    {
        var user = _entityBuilder.User.Generate();
        var request = _modelBuilder.UpdateUserCommandRequest
            .RuleFor(u => u.Key, user.Key)
            .Generate();

        _userCommandRepository.GetByIdAsync(user.Key, _cancellationToken).Returns(user);
        _userCommandRepository.ExistByEmailAsync(request.Email, request.Key, _cancellationToken)
            .Returns(true);

        var duplicateException = await Should.ThrowAsync<FCGDuplicateException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        duplicateException.Message
            .ShouldBe($"The email '{request.Email}' is already in use.");
        await _userCommandRepository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<UserUpdatedEvent>(),
                _cancellationToken
            );
    }

    [Theory]
    [InlineData(null, "colt@email.com", "FullName is required.")]
    [InlineData("", "colt@email.com", "FullName is required.")]
    [InlineData(" ", "colt@email.com", "FullName is required.")]
    [InlineData("Colt Macias", null, "Email is required.")]
    [InlineData("Colt Macias", "", "Email is required.")]
    [InlineData("Colt Macias", " ", "Email is required.")]
    public async Task ShouldThrowValidationExceptionAsync(
        string? fullName,
        string? email,
        string expectedMessage
    )
    {
        var request = _modelBuilder.UpdateUserCommandRequest
            .RuleFor(u => u.FullName, fullName)
            .RuleFor(u => u.Email, email)
            .Generate();

        var validationException = await Should.ThrowAsync<FCGValidationException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        validationException.Message.ShouldBe(expectedMessage);
        await _userCommandRepository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
        await _mediator
            .DidNotReceive()
            .Publish(
                Arg.Any<UserUpdatedEvent>(),
                _cancellationToken
            );
    }
}
