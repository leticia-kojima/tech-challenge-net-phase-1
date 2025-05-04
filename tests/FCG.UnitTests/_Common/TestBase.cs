namespace FCG.UnitTests._Common;
public abstract class TestBase : IClassFixture<FCGFixture>
{
    protected CancellationToken _cancellationToken;

    protected TestBase(FCGFixture fixture)
    {
        _cancellationToken = fixture.CancellationToken;
    }
}
