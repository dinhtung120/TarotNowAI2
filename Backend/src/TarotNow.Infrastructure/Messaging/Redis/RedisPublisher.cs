using System.Text.Json;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Messaging.Redis;

/// <summary>
/// Publisher phát realtime message lên Redis Pub/Sub.
/// </summary>
public sealed class RedisPublisher : IRedisPublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<RedisPublisher> _logger;

    /// <summary>
    /// Khởi tạo Redis publisher.
    /// </summary>
    public RedisPublisher(
        IConnectionMultiplexer connectionMultiplexer,
        ILogger<RedisPublisher> logger)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task PublishAsync(string channel, string eventName, object payload, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentException("Channel is required.", nameof(channel));
        }

        if (string.IsNullOrWhiteSpace(eventName))
        {
            throw new ArgumentException("Event name is required.", nameof(eventName));
        }

        var envelope = new RedisRealtimeEnvelope
        {
            EventName = eventName,
            Payload = JsonSerializer.SerializeToElement(payload, JsonOptions),
            OccurredAtUtc = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(envelope, JsonOptions);
        cancellationToken.ThrowIfCancellationRequested();
        await _connectionMultiplexer.GetSubscriber().PublishAsync(RedisChannel.Literal(channel), json);

        _logger.LogDebug("Published realtime event {EventName} to channel {Channel}", eventName, channel);
    }
}
