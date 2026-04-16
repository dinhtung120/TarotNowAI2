using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Gacha;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler publish realtime inventory.changed sau khi dùng item.
/// </summary>
public sealed class InventoryItemUsedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ItemUsedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler inventory item used realtime.
    /// </summary>
    public InventoryItemUsedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        ItemUsedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        if (domainEvent.IsIdempotentReplay)
        {
            return Task.CompletedTask;
        }

        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.UserState,
            RealtimeEventNames.InventoryChanged,
            new
            {
                userId = domainEvent.UserId.ToString(),
                itemCode = domainEvent.ItemCode,
                quantity = domainEvent.Quantity,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}

/// <summary>
/// Handler publish realtime inventory.changed khi gacha cấp item.
/// </summary>
public sealed class ItemGrantedFromGachaRealtimeHandler
    : IdempotentDomainEventNotificationHandler<ItemGrantedFromGachaDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler item granted from gacha realtime.
    /// </summary>
    public ItemGrantedFromGachaRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        ItemGrantedFromGachaDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.UserState,
            RealtimeEventNames.InventoryChanged,
            new
            {
                userId = domainEvent.UserId.ToString(),
                itemCode = domainEvent.ItemCode,
                quantity = domainEvent.QuantityGranted,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}

/// <summary>
/// Handler publish realtime inventory.changed khi card được enhancement.
/// </summary>
public sealed class CardEnhancedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<CardEnhancedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler card enhanced realtime.
    /// </summary>
    public CardEnhancedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        CardEnhancedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.UserState,
            RealtimeEventNames.InventoryChanged,
            new
            {
                userId = domainEvent.UserId.ToString(),
                cardId = domainEvent.CardId,
                enhancementType = domainEvent.EnhancementType,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}

