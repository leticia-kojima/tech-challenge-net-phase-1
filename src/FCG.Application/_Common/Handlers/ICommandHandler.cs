using FCG.Application._Common.Contracts;

namespace FCG.Application._Common.Handlers;
public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
}
