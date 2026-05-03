using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Chuyển EscrowReleasedDomainEvent thành MoneyChangedDomainEvent để đồng bộ pipeline ví.
/// </summary>
public sealed class EscrowReleasedMoneyChangedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<EscrowReleasedDomainEvent>
{
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler ánh xạ escrow release sang wallet money-changed.
    /// </summary>
    public EscrowReleasedMoneyChangedDomainEventHandler(
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        EscrowReleasedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = domainEvent.ReceiverId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.EscrowRelease,
                DeltaAmount = domainEvent.ReleasedAmountDiamond,
                ReferenceId = domainEvent.ItemId.ToString()
            },
            cancellationToken);

        // Payer nhận tín hiệu refresh số dư/frozen dù số dư khả dụng có thể không đổi.
        await _domainEventPublisher.PublishAsync(
            new WalletSnapshotChangedDomainEvent
            {
                UserId = domainEvent.PayerId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.EscrowRelease,
                ReferenceId = domainEvent.ItemId.ToString()
            },
            cancellationToken);
    }
}

/// <summary>
/// Chuyển EscrowSessionReleasedDomainEvent thành MoneyChangedDomainEvent để đồng bộ pipeline ví theo session-level payout.
/// </summary>
public sealed class EscrowSessionReleasedMoneyChangedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<EscrowSessionReleasedDomainEvent>
{
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler ánh xạ escrow session release sang wallet money-changed.
    /// </summary>
    public EscrowSessionReleasedMoneyChangedDomainEventHandler(
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        EscrowSessionReleasedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = domainEvent.ReceiverId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.EscrowRelease,
                DeltaAmount = domainEvent.ReleasedAmountDiamond,
                ReferenceId = domainEvent.FinanceSessionId.ToString()
            },
            cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new WalletSnapshotChangedDomainEvent
            {
                UserId = domainEvent.PayerId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.EscrowRelease,
                ReferenceId = domainEvent.FinanceSessionId.ToString()
            },
            cancellationToken);
    }
}

/// <summary>
/// Chuyển EscrowRefundedDomainEvent thành MoneyChangedDomainEvent để đồng bộ pipeline ví.
/// </summary>
public sealed class EscrowRefundedMoneyChangedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<EscrowRefundedDomainEvent>
{
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler ánh xạ escrow refund sang wallet money-changed.
    /// </summary>
    public EscrowRefundedMoneyChangedDomainEventHandler(
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        EscrowRefundedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = domainEvent.UserId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.EscrowRefund,
                DeltaAmount = domainEvent.AmountDiamond,
                ReferenceId = domainEvent.ItemId.ToString()
            },
            cancellationToken);
    }
}
