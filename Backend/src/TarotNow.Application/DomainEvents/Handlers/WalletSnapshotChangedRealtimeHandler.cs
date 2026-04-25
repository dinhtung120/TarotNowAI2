using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler publish realtime khi snapshot ví thay đổi nhưng không có delta số dư.
/// </summary>
public sealed class WalletSnapshotChangedRealtimeHandler
    : IdempotentDomainEventNotificationHandler<WalletSnapshotChangedDomainEvent>
{
    private readonly IRedisPublisher _redisPublisher;

    public WalletSnapshotChangedRealtimeHandler(
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _redisPublisher = redisPublisher;
    }

    protected override Task HandleDomainEventAsync(
        WalletSnapshotChangedDomainEvent domainEvent,
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
                deltaAmount = (long?)null,
                referenceId = domainEvent.ReferenceId
            },
            cancellationToken);
    }
}
