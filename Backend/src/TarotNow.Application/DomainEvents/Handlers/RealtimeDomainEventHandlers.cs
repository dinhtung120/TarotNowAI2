using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;
using TarotNow.Domain.Events.Gacha;

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
/// Handler publish realtime gacha.result event.
/// </summary>
public sealed class GachaPullCompletedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<GachaPullCompletedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler gacha pull completed realtime.
    /// </summary>
    public GachaPullCompletedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        GachaPullCompletedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _redisPublisher.PublishAsync(
            RealtimeChannelNames.Gacha,
            RealtimeEventNames.GachaResult,
            new
            {
                userId = domainEvent.UserId.ToString(),
                poolCode = domainEvent.PoolCode,
                pullCount = domainEvent.PullCount,
                wasPityTriggered = domainEvent.WasPityTriggered,
                at = domainEvent.OccurredAtUtc
            },
            cancellationToken);
    }
}
