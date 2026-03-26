using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.DomainEvents.Notifications;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed class EscrowReleasedNotificationHandler : INotificationHandler<EscrowReleasedNotification>
{
    private readonly ILogger<EscrowReleasedNotificationHandler> _logger;

    public EscrowReleasedNotificationHandler(ILogger<EscrowReleasedNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(EscrowReleasedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        _logger.LogInformation(
            "[DomainEvent] Escrow released. ItemId={ItemId}, PayerId={PayerId}, ReceiverId={ReceiverId}, Gross={Gross}, Released={Released}, Fee={Fee}, Auto={Auto}",
            domainEvent.ItemId,
            domainEvent.PayerId,
            domainEvent.ReceiverId,
            domainEvent.GrossAmountDiamond,
            domainEvent.ReleasedAmountDiamond,
            domainEvent.FeeAmountDiamond,
            domainEvent.IsAutoRelease);

        return Task.CompletedTask;
    }
}

public sealed class EscrowRefundedNotificationHandler : INotificationHandler<EscrowRefundedNotification>
{
    private readonly ILogger<EscrowRefundedNotificationHandler> _logger;

    public EscrowRefundedNotificationHandler(ILogger<EscrowRefundedNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(EscrowRefundedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        _logger.LogInformation(
            "[DomainEvent] Escrow refunded. ItemId={ItemId}, UserId={UserId}, Amount={Amount}, Source={Source}",
            domainEvent.ItemId,
            domainEvent.UserId,
            domainEvent.AmountDiamond,
            domainEvent.RefundSource);

        return Task.CompletedTask;
    }
}

public sealed class ReadingBillingCompletedNotificationHandler : INotificationHandler<ReadingBillingCompletedNotification>
{
    private readonly ILogger<ReadingBillingCompletedNotificationHandler> _logger;

    public ReadingBillingCompletedNotificationHandler(ILogger<ReadingBillingCompletedNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ReadingBillingCompletedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        _logger.LogInformation(
            "[DomainEvent] Reading billing finalized. AiRequestId={AiRequestId}, UserId={UserId}, SessionRef={SessionRef}, Charge={Charge}, Status={Status}, Refunded={Refunded}",
            domainEvent.AiRequestId,
            domainEvent.UserId,
            domainEvent.ReadingSessionRef,
            domainEvent.ChargeDiamond,
            domainEvent.FinalStatus,
            domainEvent.WasRefunded);

        return Task.CompletedTask;
    }
}
