namespace FCG.UnitTests.Auth;
public class PasswordTests : TestBase
{
    public PasswordTests(FCGFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void ShouldValidatePasswordMatching()
    {
        var plainPassword = "3SfE@43NR1#b";
        var password = new Password(plainPassword);

        password.Matches(plainPassword).ShouldBeTrue();
        password.Matches("WrongPassword!").ShouldBeFalse();
    }

    [Fact]
    public void ShouldGenerateDifferentHashesForSamePassword()
    {
        var password = "3SfE@43NR1#b";
        var password1 = new Password(password);
        var password2 = new Password(password);

        password1.Hash.ShouldNotBe(password2.Hash);
    }

    [Theory]
    [InlineData("12345678", "Password must contain letters, numbers and special characters.")]
    [InlineData("1234", "Password must be at least 8 characters long.")]
    public void ShouldThrowExceptionForInvalidOrShortPassword(
        string password,
        string expectedMessage
    )
    {
        var validationException = Should.Throw<FCGValidationException>(
            () => new Password(password)
        );

        validationException.Message.ShouldBe(expectedMessage);
    }
}
