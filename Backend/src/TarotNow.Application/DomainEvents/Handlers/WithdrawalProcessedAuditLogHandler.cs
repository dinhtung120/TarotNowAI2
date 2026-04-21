using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Ghi log audit cho luồng xử lý yêu cầu rút tiền.
/// </summary>
public sealed class WithdrawalProcessedAuditLogHandler
    : IdempotentDomainEventNotificationHandler<WithdrawalProcessedDomainEvent>
{
    private readonly ILogger<WithdrawalProcessedAuditLogHandler> _logger;

    /// <summary>
    /// Khởi tạo audit log handler cho withdrawal processed.
    /// </summary>
    public WithdrawalProcessedAuditLogHandler(
        ILogger<WithdrawalProcessedAuditLogHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        WithdrawalProcessedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[WithdrawalAudit] Request processed. RequestId={RequestId}, UserId={UserId}, AdminId={AdminId}, Action={Action}, Status={Status}, AmountDiamond={AmountDiamond}, ProcessedAtUtc={ProcessedAtUtc}",
            domainEvent.RequestId,
            domainEvent.UserId,
            domainEvent.AdminId,
            domainEvent.Action,
            domainEvent.Status,
            domainEvent.AmountDiamond,
            domainEvent.ProcessedAtUtc);
        return Task.CompletedTask;
    }
}
