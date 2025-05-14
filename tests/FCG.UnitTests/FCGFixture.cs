using AutoBogus.Conventions;
using FCG.Domain._Common.Abstract;
using FCG.UnitTests.Builders;

namespace FCG.UnitTests;
public class FCGFixture : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    public FCGFixture()
    {
        _cancellationTokenSource = new();
        EntityBuilder = new();
        ModelBuilder = new();
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

    public void Dispose() => _cancellationTokenSource.Cancel();
}
