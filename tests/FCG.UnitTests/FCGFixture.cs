using AutoBogus;

namespace FCG.UnitTests;
public class FCGFixture : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    public FCGFixture()
    {
        _cancellationTokenSource = new();
        CancellationToken = _cancellationTokenSource.Token;

        AutoFaker.Configure(builder =>
        {
            builder
              .WithLocale("pt_BR");
        });
    }

    public CancellationToken CancellationToken { get; private set; }

    public void Dispose() => _cancellationTokenSource.Cancel();
}
