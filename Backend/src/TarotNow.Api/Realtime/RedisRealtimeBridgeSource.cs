using System.Collections.Concurrent;
using StackExchange.Redis;

namespace TarotNow.Api.Realtime;

/// <summary>
/// Source realtime dựa trên Redis Pub/Sub.
/// </summary>
public sealed class RedisRealtimeBridgeSource : IRealtimeBridgeSource
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisRealtimeBridgeSource> _logger;
    private readonly ConcurrentDictionary<string, Action<RedisChannel, RedisValue>> _subscriptions = new(StringComparer.Ordinal);

    /// <summary>
    /// Khởi tạo Redis realtime source.
    /// </summary>
    public RedisRealtimeBridgeSource(
        IConnectionMultiplexer connectionMultiplexer,
        ILogger<RedisRealtimeBridgeSource> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
    }

    /// <inheritdoc />
    public bool IsEnabled => true;

    /// <inheritdoc />
    public async Task SubscribeAsync(
        string channel,
        Func<string, string, Task> onMessageAsync,
        CancellationToken cancellationToken = default)
    {
        ValidateArguments(channel, onMessageAsync);

        if (_subscriptions.ContainsKey(channel))
        {
            return;
        }

        var callback = CreateRedisCallback(onMessageAsync);
        if (_subscriptions.TryAdd(channel, callback) == false)
        {
            return;
        }

        var subscriber = _connectionMultiplexer.GetSubscriber();
        await subscriber.SubscribeAsync(RedisChannel.Literal(channel), callback);
        _logger.LogInformation("Subscribed Redis realtime channel {Channel}.", channel);
    }

    /// <inheritdoc />
    public async Task UnsubscribeAsync(string channel, CancellationToken cancellationToken = default)
    {
        if (_subscriptions.TryRemove(channel, out var callback) == false)
        {
            return;
        }

        var subscriber = _connectionMultiplexer.GetSubscriber();
        await subscriber.UnsubscribeAsync(RedisChannel.Literal(channel), callback);
        _logger.LogInformation("Unsubscribed Redis realtime channel {Channel}.", channel);
    }

    private Action<RedisChannel, RedisValue> CreateRedisCallback(Func<string, string, Task> onMessageAsync)
    {
        return (redisChannel, redisValue) =>
        {
            try
            {
                // Chạy callback theo kiểu blocking tại điểm subscribe để giữ backpressure và không nuốt exception.
                onMessageAsync(redisChannel.ToString(), redisValue.ToString()).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Redis realtime callback failed. Channel={Channel}",
                    redisChannel.ToString());
            }
        };
    }

    private static void ValidateArguments(string channel, Func<string, string, Task> onMessageAsync)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentException("Channel is required.", nameof(channel));
        }

        ArgumentNullException.ThrowIfNull(onMessageAsync);
    }
}
