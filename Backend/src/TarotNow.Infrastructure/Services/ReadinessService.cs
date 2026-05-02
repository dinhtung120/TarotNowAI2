using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using StackExchange.Redis;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Services;

// Service kiểm tra readiness dependency để presentation layer không truy cập DB trực tiếp.
public sealed class ReadinessService : IReadinessService
{
    private static readonly string[] RequiredPostgreSqlTables = ["users", "gacha_pools", "auth_sessions"];
    private static readonly string[] RequiredMongoCollections = ["cards_catalog", "quests", "achievements", "titles", "refresh_tokens"];

    private readonly ApplicationDbContext _dbContext;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly CacheBackendState _cacheBackendState;
    private readonly IConnectionMultiplexer? _redisConnectionMultiplexer;
    private readonly ILogger<ReadinessService> _logger;
    private readonly bool _redisRequiredForPresence;

    public ReadinessService(
        ApplicationDbContext dbContext,
        IMongoDatabase mongoDatabase,
        CacheBackendState cacheBackendState,
        IHostEnvironment hostEnvironment,
        ILogger<ReadinessService> logger,
        IConnectionMultiplexer? redisConnectionMultiplexer = null)
    {
        _dbContext = dbContext;
        _mongoDatabase = mongoDatabase;
        _cacheBackendState = cacheBackendState;
        _logger = logger;
        _redisConnectionMultiplexer = redisConnectionMultiplexer;
        _redisRequiredForPresence = !IsLocalPresenceFallbackAllowed(hostEnvironment.EnvironmentName);
    }

    /// <summary>
    /// Kiểm tra trạng thái dependency chính gồm PostgreSQL, MongoDB và Redis.
    /// </summary>
    public async Task<ReadinessStatus> CheckAsync(CancellationToken cancellationToken = default)
    {
        var postgresReady = await CheckPostgreSqlAsync(cancellationToken);
        var mongoReady = await CheckMongoDbAsync(cancellationToken);
        var redisReady = await CheckRedisAsync();
        var redisRequired = _cacheBackendState.UsesRedis || _redisRequiredForPresence;

        return new ReadinessStatus(postgresReady, mongoReady, redisReady, redisRequired);
    }

    private static bool IsLocalPresenceFallbackAllowed(string environmentName)
    {
        return environmentName.Equals("Development", StringComparison.OrdinalIgnoreCase)
            || environmentName.Equals("Local", StringComparison.OrdinalIgnoreCase)
            || environmentName.Equals("Test", StringComparison.OrdinalIgnoreCase)
            || environmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase);
    }

    private async Task<bool> CheckPostgreSqlAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                return false;
            }

            var requiredTableCount = RequiredPostgreSqlTables.Length;
            var query = """
                        SELECT COUNT(*)
                        FROM information_schema.tables
                        WHERE table_schema = 'public'
                          AND table_name IN ('users', 'gacha_pools', 'auth_sessions')
                        """;

            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            await using var command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;

            var result = await command.ExecuteScalarAsync(cancellationToken);
            var existingTableCount = Convert.ToInt32(result);

            if (existingTableCount < requiredTableCount)
            {
                _logger.LogWarning(
                    "Readiness check failed for PostgreSQL: missing required tables. Required={RequiredCount}, Existing={ExistingCount}.",
                    requiredTableCount,
                    existingTableCount);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Readiness check failed for PostgreSQL.");
            return false;
        }
    }

    private async Task<bool> CheckMongoDbAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mongoDatabase.RunCommandAsync<BsonDocument>(
                new BsonDocument("ping", 1),
                cancellationToken: cancellationToken);

            var pingOk = result.Contains("ok") && result["ok"].ToDouble() >= 1;
            if (!pingOk)
            {
                return false;
            }

            var existingCollections = await _mongoDatabase.ListCollectionNames(cancellationToken: cancellationToken).ToListAsync(cancellationToken);
            var missingCollections = RequiredMongoCollections
                .Where(requiredCollection => !existingCollections.Contains(requiredCollection, StringComparer.Ordinal))
                .ToArray();

            if (missingCollections.Length > 0)
            {
                _logger.LogWarning(
                    "Readiness check failed for MongoDB: missing required collections {MissingCollections}.",
                    string.Join(", ", missingCollections));
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Readiness check failed for MongoDB.");
            return false;
        }
    }

    private async Task<bool> CheckRedisAsync()
    {
        try
        {
            if (!_cacheBackendState.UsesRedis || _redisConnectionMultiplexer is null)
            {
                return false;
            }

            var database = _redisConnectionMultiplexer.GetDatabase();
            var ping = await database.PingAsync();
            return ping >= TimeSpan.Zero;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Readiness check failed for Redis.");
            return false;
        }
    }
}
