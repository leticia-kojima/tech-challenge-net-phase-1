using FCG.Application.Commands.Users;
using FCG.Domain._Common.Exceptions;
using FCG.Domain.Users;

namespace FCG.UnitTests.Users;
public class UpdateUserCommandHandlerTests : TestHandlerBase<UpdateUserCommandHandler>
{
    private readonly IUserCommandRepository _repository;

    public UpdateUserCommandHandlerTests(FCGFixture fixture) : base(fixture)
    {
        _repository = GetMock<IUserCommandRepository>();
    }

    [Fact]
    public async Task ShouldUpdateUserAsync()
    {
        var user = _entityBuilder.User.Generate();
        var request = _modelBuilder.UpdateUserCommandRequest
            .RuleFor(u => u.Key, user.Key)
            .Generate();

        _repository.GetByIdAsync(user.Key, _cancellationToken).Returns(user);

        var result = await Handler.Handle(request, _cancellationToken);

        result.Key.ShouldNotBe(Guid.Empty);
        result.FullName.ShouldBe(request.FullName);
        result.Email.ShouldBe(request.Email);
        result.Role.ShouldBe(request.Role);
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

    [Fact]
    public async Task ShouldThrowExcpetionForUserDuplicationAsync()
    {
        var user = _entityBuilder.User.Generate();
        var request = _modelBuilder.UpdateUserCommandRequest
            .RuleFor(u => u.Key, user.Key)
            .Generate();

        _repository.GetByIdAsync(user.Key, _cancellationToken).Returns(user);
        _repository.ExistByEmailAsync(request.Email, request.Key, _cancellationToken)
            .Returns(true);

        var duplicateException = await Should.ThrowAsync<FCGDuplicateException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        duplicateException.ShouldNotBeNull();
        duplicateException.Message.ShouldBe("An user with this email already exists.");
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
        var request = _modelBuilder.UpdateUserCommandRequest
            .RuleFor(u => u.FullName, fullName)
            .RuleFor(u => u.Email, email)
            .RuleFor(u => u.Role, role)
            .Generate();

        var validationException = await Should.ThrowAsync<FCGValidationException>(
            () => Handler.Handle(request, _cancellationToken)
        );

        validationException.ShouldNotBeNull();
        validationException.Message.ShouldBe(expectedMessage);
        await _repository
            .DidNotReceive()
            .UpdateAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
    }
}
