using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler publish realtime wallet change event.
/// </summary>
public sealed class MoneyChangedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<MoneyChangedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler wallet realtime.
    /// </summary>
    public MoneyChangedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        MoneyChangedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.Wallet,
            RealtimeEventNames.WalletBalanceChanged,
            new
            {
                userId = domainEvent.UserId.ToString(),
                currency = domainEvent.Currency,
                changeType = domainEvent.ChangeType,
                deltaAmount = domainEvent.DeltaAmount,
                referenceId = domainEvent.ReferenceId
            },
            cancellationToken);
    }
}

/// <summary>
/// Handler publish realtime conversation.updated event.
/// </summary>
public sealed class ConversationUpdatedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ConversationUpdatedDomainEvent>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler conversation updated realtime.
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
        if (conversation == null)
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
/// Handler publish realtime message.created event.
/// </summary>
public sealed class ChatMessageCreatedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ChatMessageCreatedDomainEvent>
{
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler message created realtime.
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
        if (message == null)
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
/// Handler publish realtime unread changed event.
/// </summary>
public sealed class UnreadCountChangedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<UnreadCountChangedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler unread changed realtime.
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
/// Handler publish realtime gacha.result event.
/// </summary>
public sealed class GachaSpunRealtimeHandler
    : IdempotentDomainEventNotificationHandler<GachaSpunDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler gacha spun realtime.
    /// </summary>
    public GachaSpunRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        GachaSpunDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.Gacha,
            RealtimeEventNames.GachaResult,
            new
            {
                userId = domainEvent.UserId.ToString(),
                bannerCode = domainEvent.BannerCode,
                spinCount = domainEvent.SpinCount,
                wasPityTriggered = domainEvent.WasPityTriggered,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}
