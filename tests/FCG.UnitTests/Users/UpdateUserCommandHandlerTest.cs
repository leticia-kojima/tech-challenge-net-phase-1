using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Commands;
using FCG.Domain._Common;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class UpdateUserCommandHandlerTest : TestBase
{
    private readonly IUserCommandRepository _repository;
    private readonly IMediator _mediator;

    public UpdateUserCommandHandlerTest(FCGFixture fixture) : base(fixture)
    {
        _repository = Substitute.For<IUserCommandRepository>();
        _mediator = Substitute.For<IMediator>();
    }

    [Fact]
    public async Task ShouldUpdateUserAsync()
    {
        var user = AutoFaker.Generate<User>();
        _repository.GetByIdAsync(user.Key, _cancellationToken).Returns(user);
        var request = AutoFaker.Generate<UpdateUserCommandRequest>();

        request.Key = user.Key;
        request.FullName = "Colt Macias";
        request.Email = "colt@email.com";
        request.Role = ERole.User;

        var command = new UpdateUserCommandHandler(_repository, _mediator);

        var result = await command.Handle(request, _cancellationToken);

        Assert.NotEqual(Guid.Empty, result.Key);
        Assert.Equal(request.FullName, result.FullName);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.Role, result.Role);
        await _repository
            .Received(1)
            .UpdateAsync(
                Arg.Is<User>(u => u.Key == request.Key
                    && u.FullName == request.FullName
                    && u.Email.ToString() == request.Email
                    && u.Role == request.Role),
                _cancellationToken
            );
    }

    [Theory]
    [InlineData(null, "colt@email.com", ERole.User, "FullName is required.")]
    [InlineData("", "colt@email.com", ERole.User, "FullName is required.")]
    [InlineData(" ", "colt@email.com", ERole.User, "FullName is required.")]
    [InlineData("Colt Macias", null, ERole.User, "Email is required.")]
    [InlineData("Colt Macias", "", ERole.User, "Email is required.")]
    [InlineData("Colt Macias", " ", ERole.User, "Email is required.")]

    public async Task ShouldThrowValidationExceptionAsync(
        string? fullName,
        string? email,
        ERole role,
        string expectedMessage
    )
    {
        var request = new UpdateUserCommandRequest
        {
            FullName = fullName,
            Email = email,
            Role = role,
        };
        var command = new UpdateUserCommandHandler(_repository, _mediator);

        var validationException = await Assert.ThrowsAsync<FCGValidationException>(
            () => command.Handle(request, _cancellationToken)
        );

        Assert.NotNull(validationException);
        Assert.Equal(expectedMessage, validationException.Message);
        await _repository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
    }
}
