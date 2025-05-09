namespace FCG.Application._Common.Contracts;
public interface IQueryList<out TModel> :
    IQuery<IReadOnlyCollection<TModel>>,
    IRequest<IReadOnlyCollection<TModel>>
{
}
