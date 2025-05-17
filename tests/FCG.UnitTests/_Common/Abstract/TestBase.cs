namespace FCG.UnitTests._Common.Abstract;
public abstract class TestBase : IClassFixture<FCGFixture>
{
    protected readonly CancellationToken _cancellationToken;
    protected readonly FCGEntityBuilder _entityBuilder;
    protected readonly FCGModelBuilder _modelBuilder;
    protected readonly Faker _faker;

    protected TestBase(FCGFixture fixture)
    {
        _cancellationToken = fixture.CancellationToken;
        _entityBuilder = fixture.EntityBuilder;
        _modelBuilder = fixture.ModelBuilder;
        _faker = fixture.Faker;
    }
}
