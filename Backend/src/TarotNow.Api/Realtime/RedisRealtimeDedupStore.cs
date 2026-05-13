using StackExchange.Redis;

namespace TarotNow.Api.Realtime;

public sealed class RedisRealtimeDedupStore : IRealtimeDedupStore
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisRealtimeDedupStore> _logger;

    public RedisRealtimeDedupStore(
        IConnectionMultiplexer connectionMultiplexer,
        ILogger<RedisRealtimeDedupStore> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
    }

    public async Task<bool> TryClaimAsync(string eventId, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        try
        {
            var database = _connectionMultiplexer.GetDatabase();
            return await database.StringSetAsync($"realtime:dedup:{eventId}", "1", ttl, When.NotExists);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Realtime distributed dedup unavailable. EventId={EventId}", eventId);
            return false;
        }
    }
}
