using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler publish realtime message.read event.
/// </summary>
public sealed class ChatMessageReadRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ChatMessageReadDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler message read realtime.
    /// </summary>
    public ChatMessageReadRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        ChatMessageReadDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.Chat,
            RealtimeEventNames.ChatMessageRead,
            new
            {
                userId = domainEvent.UserId,
                conversationId = domainEvent.ConversationId,
                readAt = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}

/// <summary>
/// Handler publish realtime typing.started và typing.stopped event.
/// </summary>
public sealed class ChatTypingChangedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ChatTypingChangedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler typing changed realtime.
    /// </summary>
    public ChatTypingChangedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        ChatTypingChangedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var eventName = domainEvent.IsTyping
            ? RealtimeEventNames.TypingStarted
            : RealtimeEventNames.TypingStopped;

        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.Chat,
            eventName,
            new
            {
                conversationId = domainEvent.ConversationId,
                userId = domainEvent.UserId,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}
