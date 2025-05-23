using Dapper;
using FCG.Domain.Games;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;

namespace FCG.Infrastructure.Repositories.Games;

public class DapperGameQueryRepository : IGameQueryRepository
{
    private readonly FCGCommandsDbContext _context;
    private readonly string _connectionString;

    public DapperGameQueryRepository(FCGCommandsDbContext context)
    {
        _context = context;
        _connectionString = _context.Database.GetConnectionString();
    }

    #region IGameQueryRepository Implementation

    public async Task<Game?> GetByKeyAsync(Guid key, CancellationToken? cancellationToken = null)
    {
        using IDbConnection db = new MySqlConnection(_connectionString);
        
        var query = @"
            SELECT g.*, c.*
            FROM Games g
            LEFT JOIN Catalogs c ON g.CatalogKey = c.Key
            WHERE g.Key = @Key";
            
        var games = await db.QueryAsync<Game, Domain.Catalogs.Catalog, Game>(
            query,
            (game, catalog) =>
            {
                return game;
            },
            new { Key = key },
            splitOn: "Key");
            
        return games.FirstOrDefault();
    }

    public async Task<IEnumerable<Game>> GetAllAsync(CancellationToken? cancellationToken = null)
    {
        using IDbConnection db = new MySqlConnection(_connectionString);
        
        var query = @"
            SELECT g.*, c.* 
            FROM Games g
            LEFT JOIN Catalogs c ON g.CatalogKey = c.Key
            ORDER BY g.Title";

        var gameDictionary = new Dictionary<Guid, Game>();
        
        var games = await db.QueryAsync<Game, Domain.Catalogs.Catalog, Game>(
            query,
            (game, catalog) =>
            {
                if (!gameDictionary.TryGetValue(game.Key, out var gameEntry))
                {
                    gameEntry = game;
                    gameDictionary.Add(game.Key, gameEntry);
                }
                
                return game;
            },
            splitOn: "Key");
            
        return gameDictionary.Values;
    }

    public async Task<IEnumerable<Game>> GetByCatalogKeyAsync(Guid catalogKey, CancellationToken? cancellationToken = null)
    {
        using IDbConnection db = new MySqlConnection(_connectionString);
        
        var query = @"
            SELECT g.*, c.*
            FROM Games g
            LEFT JOIN Catalogs c ON g.CatalogKey = c.Key
            WHERE g.CatalogKey = @CatalogKey
            ORDER BY g.Title";

        var gameDictionary = new Dictionary<Guid, Game>();
        
        var games = await db.QueryAsync<Game, Domain.Catalogs.Catalog, Game>(
            query,
            (game, catalog) =>
            {
                if (!gameDictionary.TryGetValue(game.Key, out var gameEntry))
                {
                    gameEntry = game;
                    gameDictionary.Add(game.Key, gameEntry);
                }
                
                return game;
            },
            new { CatalogKey = catalogKey },
            splitOn: "Key");
            
        return gameDictionary.Values;
    }

    #endregion

    #region IRepository<Game> Implementation

    public async Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        // Reusing the GetByKeyAsync method to keep the code DRY
        return await GetByKeyAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Game>> GetAllAsync(CancellationToken cancellationToken)
    {
        // Reusing the GetAllAsync method with optional parameter
        var games = await GetAllAsync((CancellationToken?)cancellationToken);
        return games.ToList().AsReadOnly();
    }

    public Task AddAsync(Game entity, CancellationToken cancellationToken)
    {
        // we delegate write operations to the command repository
        throw new NotSupportedException("This repository is for queries only. Use the command repository to add entities.");
    }

    public Task UpdateAsync(Game entity, CancellationToken cancellationToken)
    {
        // we delegate write operations to the command repository
        throw new NotSupportedException("This repository is for queries only. Use the command repository to update entities.");
    }

    public Task DeleteAsync(Game entity, CancellationToken cancellationToken)
    {
        // we delegate write operations to the command repository
        throw new NotSupportedException("This repository is for queries only. Use the command repository to delete entities.");
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        // we delegate write operations to the command repository
        throw new NotSupportedException("This repository is for queries only. Use the command repository to delete entities.");
    }

    #endregion
}
