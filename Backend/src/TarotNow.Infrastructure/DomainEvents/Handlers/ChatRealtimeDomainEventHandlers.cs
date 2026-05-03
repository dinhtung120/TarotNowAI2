using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.DomainEvents.Handlers;

/// <summary>
/// Publish conversation delta từ domain events sang Redis chat channel.
/// </summary>
public sealed class ConversationUpdatedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ConversationUpdatedDomainEvent>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo realtime handler cho conversation updated.
    /// </summary>
    public ConversationUpdatedRealtimeHandler(
        IConversationRepository conversationRepository,
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _conversationRepository = conversationRepository;
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ConversationUpdatedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(domainEvent.ConversationId, cancellationToken);
        if (conversation is null)
        {
            return;
        }

        await _redisPublisher.PublishAsync(
            RealtimeChannelNames.Chat,
            RealtimeEventNames.ConversationUpdated,
            new
            {
                conversationId = domainEvent.ConversationId,
                type = domainEvent.Type,
                at = domainEvent.OccurredAtUtc,
                userId = conversation.UserId,
                readerId = conversation.ReaderId
            },
            cancellationToken);
    }
}

/// <summary>
/// Publish message.created từ domain events sang Redis chat channel.
/// </summary>
public sealed class ChatMessageCreatedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ChatMessageCreatedDomainEvent>
{
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo realtime handler cho message created.
    /// </summary>
    public ChatMessageCreatedRealtimeHandler(
        IChatMessageRepository chatMessageRepository,
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _chatMessageRepository = chatMessageRepository;
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ChatMessageCreatedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var message = await _chatMessageRepository.GetByIdAsync(domainEvent.MessageId, cancellationToken);
        if (message is null)
        {
            return;
        }

        await _redisPublisher.PublishAsync(
            RealtimeChannelNames.Chat,
            RealtimeEventNames.ChatMessageCreated,
            new
            {
                conversationId = domainEvent.ConversationId,
                message
            },
            cancellationToken);
    }
}

/// <summary>
/// Publish unread aggregate change sang Redis chat channel.
/// </summary>
public sealed class UnreadCountChangedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<UnreadCountChangedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo realtime handler cho unread count changed.
    /// </summary>
    public UnreadCountChangedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        UnreadCountChangedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.Chat,
            RealtimeEventNames.ChatUnreadChanged,
            new
            {
                conversationId = domainEvent.ConversationId,
                userId = domainEvent.UserId,
                readerId = domainEvent.ReaderId,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}

/// <summary>
/// Publish message.read sang Redis chat channel để đồng bộ đa thiết bị.
/// </summary>
public sealed class ChatMessageReadRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ChatMessageReadDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo realtime handler cho message read.
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
/// Publish typing started/stopped event sang Redis chat channel.
/// </summary>
public sealed class ChatTypingChangedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ChatTypingChangedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo realtime handler cho typing changed.
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
