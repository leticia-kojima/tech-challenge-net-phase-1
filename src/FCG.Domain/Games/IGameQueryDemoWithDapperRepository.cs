using FCG.Domain.Games.Contracts;

namespace FCG.Domain.Games;
public interface IGameQueryDemoWithDapperRepository : IRepository
{
    Task<IReadOnlyCollection<EvaluationView>> GetEvaluationsAsync(
        Guid catalogKey,
        Guid gameKey,
        CancellationToken cancellationToken
    );
}
