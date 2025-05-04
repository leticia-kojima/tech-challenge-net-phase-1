using AutoBogus;
using FCG.Application.Commands.Users;
using FCG.Application.Contracts.Users.Commands;
using FCG.Domain._Common;
using FCG.Domain.Users;
using FCG.UnitTests._Common;
using MediatR;
using NSubstitute;

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
        var command = new CreateUserCommandHandler(_repository, _mediator);

        var result = await command.Handle(request, _cancellationToken);

        Assert.NotEqual(Guid.Empty, result.Key);
        Assert.Equal(request.FirstName, result.FirstName);
        Assert.Equal(request.LastName, result.LastName);
        await _repository
            .Received(1)
            .AddAsync(
                Arg.Any<User>(),
                _cancellationToken
            );
    }

    [Theory]
    [InlineData(null, "Macias", "FirstName is required.")]
    [InlineData("", "Macias", "FirstName is required.")]
    [InlineData(" ", "Macias", "FirstName is required.")]
    [InlineData("Colt", null, "LastName is required.")]
    [InlineData("Colt", "", "LastName is required.")]
    [InlineData("Colt", " ", "LastName is required.")]
    // This is a demo unit test!
    public async Task ShouldThrowValidationExceptionAsync(
        string? firstName,
        string? lastName,
        string expectedMessage
    )
    {
        var request = new CreateUserCommandRequest { FirstName = firstName, LastName = lastName };
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
