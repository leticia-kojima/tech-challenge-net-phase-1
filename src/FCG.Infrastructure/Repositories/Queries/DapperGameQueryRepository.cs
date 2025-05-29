using Dapper;
using FCG.Domain.Games;
using FCG.Domain.Games.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FCG.Infrastructure.Repositories.Queries;

public class DapperGameQueryRepository : IGameQueryDemoWithDapperRepository
{
    private readonly IDbConnection _dbConnection;

    public DapperGameQueryRepository(FCGCommandsDbContext context)
    {
        _dbConnection = context.Database.GetDbConnection();
    }

    public async Task<IReadOnlyCollection<EvaluationView>> GetEvaluationsAsync(
        Guid catalogKey,
        Guid gameKey,
        CancellationToken cancellationToken)
    {
        const string query = @"
            SELECT 
                e.Key, 
                e.Stars, 
                e.Comment, 
                e.UserKey, 
                u.FullName AS UserFullName
            FROM GameEvaluations e
            INNER JOIN Users u ON e.UserKey = u.Key
            WHERE e.GameKey = @GameKey
              AND e.CatalogKey = @CatalogKey";

        var parameters = new
        {
            CatalogKey = catalogKey,
            GameKey = gameKey
        };

        var command = new CommandDefinition(
            query,
            parameters,
            cancellationToken: cancellationToken
        );

        return [.. await _dbConnection.QueryAsync<EvaluationView>(command)];
    }
}
