using FCG.UnitTests.Builders;

namespace FCG.UnitTests._Common.Abstract;
public abstract class TestBase : IClassFixture<FCGFixture>
{
    protected CancellationToken _cancellationToken;
    protected FCGEntityBuilder _entityBuilder;
    protected FCGModelBuilder _modelBuilder;

    protected TestBase(FCGFixture fixture)
    {
        _cancellationToken = fixture.CancellationToken;
        _entityBuilder = fixture.EntityBuilder;
        _modelBuilder = fixture.ModelBuilder;
    }
}
