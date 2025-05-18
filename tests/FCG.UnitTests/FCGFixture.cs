using AutoBogus.Conventions;

namespace FCG.UnitTests;
public class FCGFixture : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    public FCGFixture()
    {
        _cancellationTokenSource = new();
        EntityBuilder = new();
        ModelBuilder = new();
        Faker = new();
        CancellationToken = _cancellationTokenSource.Token;

        AutoFaker.Configure(builder =>
        {
            builder
                .WithLocale("pt_BR")
                .WithConventions();
        });
    }

    public CancellationToken CancellationToken { get; private set; }

    public FCGEntityBuilder EntityBuilder { get; private set; }

    public FCGModelBuilder ModelBuilder { get; private set; }

    public Faker Faker { get; private set; }

    public void Dispose() => _cancellationTokenSource.Cancel();
}
