using NSubstituteAutoMocker;

namespace FCG.UnitTests._Common.Abstract;
public abstract class TestHandlerBase<THandler> : TestBase
     where THandler : class
{
    protected readonly NSubstituteAutoMocker<THandler> _autoMocker;

    protected TestHandlerBase(FCGFixture fixture) : base(fixture)
        => _autoMocker = new();

    protected THandler Handler => _autoMocker.ClassUnderTest;

    protected TMock GetMock<TMock>() where TMock : class => _autoMocker.Get<TMock>();
}
