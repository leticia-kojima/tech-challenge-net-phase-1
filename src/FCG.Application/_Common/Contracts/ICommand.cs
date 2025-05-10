namespace FCG.Application._Common.Contracts;
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
