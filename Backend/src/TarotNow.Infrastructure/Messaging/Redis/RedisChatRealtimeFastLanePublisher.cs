using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Messaging.Redis;

/// <summary>
/// Publisher phát sự kiện chat fast-lane qua Redis Pub/Sub.
/// </summary>
public sealed class RedisChatRealtimeFastLanePublisher : IChatRealtimeFastLanePublisher
{
    private readonly IRedisPublisher _redisPublisher;
    private readonly ILogger<RedisChatRealtimeFastLanePublisher> _logger;

    /// <summary>
    /// Khởi tạo publisher chat fast-lane.
    /// </summary>
    public RedisChatRealtimeFastLanePublisher(
        IRedisPublisher redisPublisher,
        ILogger<RedisChatRealtimeFastLanePublisher> logger)
    {
        _redisPublisher = redisPublisher;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task PublishAsync(ChatRealtimeEnvelopeV2 envelope, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        if (string.IsNullOrWhiteSpace(envelope.EventType))
        {
            throw new ArgumentException("Fast-lane event type is required.", nameof(envelope));
        }

        if (string.IsNullOrWhiteSpace(envelope.ConversationId))
        {
            throw new ArgumentException("Fast-lane conversation id is required.", nameof(envelope));
        }

        _logger.LogDebug(
            "Publishing chat fast-lane event. EventType={EventType}, EventId={EventId}, ConversationId={ConversationId}",
            envelope.EventType,
            envelope.EventId,
            envelope.ConversationId);

        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.ChatFast,
            envelope.EventType,
            envelope,
            cancellationToken);
    }
}
