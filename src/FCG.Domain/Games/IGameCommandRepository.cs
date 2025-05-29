namespace FCG.Domain.Games;
public interface IGameCommandRepository : IGameRepository
{
    Task<bool> ExistByTitleAsync(
        string title,
        Guid? ignoreKey = null,
        CancellationToken cancellationToken = default
    );
}
