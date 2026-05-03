using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces.DomainEvents;
using Microsoft.Extensions.Logging;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

// Handler ghi log khi có sự kiện giải ngân escrow.
public sealed class EscrowReleasedNotificationHandler
    : IdempotentDomainEventNotificationHandler<EscrowReleasedDomainEvent>
{
    private readonly ILogger<EscrowReleasedNotificationHandler> _logger;

    /// <summary>
    /// Khởi tạo handler logging cho sự kiện escrow release.
    /// Luồng xử lý: nhận logger typed để ghi thông tin sự kiện tài chính phục vụ truy vết.
    /// </summary>
    public EscrowReleasedNotificationHandler(
        ILogger<EscrowReleasedNotificationHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ghi log thông tin giải ngân escrow khi notification được publish.
    /// Luồng xử lý: đọc domain event và log đầy đủ các trường cốt lõi phục vụ audit.
    /// </summary>
    protected override Task HandleDomainEventAsync(
        EscrowReleasedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        // Log đầy đủ gross/released/fee để thuận tiện đối soát tài chính và debug payout.
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

// Handler ghi log khi có sự kiện giải ngân escrow theo session-level payout.
public sealed class EscrowSessionReleasedNotificationHandler
    : IdempotentDomainEventNotificationHandler<EscrowSessionReleasedDomainEvent>
{
    private readonly ILogger<EscrowSessionReleasedNotificationHandler> _logger;

    /// <summary>
    /// Khởi tạo handler logging cho sự kiện escrow session release.
    /// Luồng xử lý: nhận logger typed để ghi thông tin payout tổng hợp theo phiên.
    /// </summary>
    public EscrowSessionReleasedNotificationHandler(
        ILogger<EscrowSessionReleasedNotificationHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ghi log thông tin giải ngân escrow theo session.
    /// Luồng xử lý: đọc domain event và log financeSession/payer/receiver/tổng tiền cho đối soát.
    /// </summary>
    protected override Task HandleDomainEventAsync(
        EscrowSessionReleasedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[DomainEvent] Escrow session released. FinanceSessionId={FinanceSessionId}, PayerId={PayerId}, ReceiverId={ReceiverId}, Gross={Gross}, Released={Released}, Fee={Fee}, ReleasedItemCount={ReleasedItemCount}, Auto={Auto}",
            domainEvent.FinanceSessionId,
            domainEvent.PayerId,
            domainEvent.ReceiverId,
            domainEvent.GrossAmountDiamond,
            domainEvent.ReleasedAmountDiamond,
            domainEvent.FeeAmountDiamond,
            domainEvent.ReleasedItemCount,
            domainEvent.IsAutoRelease);

        return Task.CompletedTask;
    }
}

// Handler ghi log khi có sự kiện hoàn tiền escrow.
public sealed class EscrowRefundedNotificationHandler
    : IdempotentDomainEventNotificationHandler<EscrowRefundedDomainEvent>
{
    private readonly ILogger<EscrowRefundedNotificationHandler> _logger;

    /// <summary>
    /// Khởi tạo handler logging cho sự kiện hoàn tiền escrow.
    /// Luồng xử lý: nhận logger để ghi audit trail giao dịch hoàn tiền.
    /// </summary>
    public EscrowRefundedNotificationHandler(
        ILogger<EscrowRefundedNotificationHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ghi log thông tin hoàn tiền escrow.
    /// Luồng xử lý: đọc domain event và ghi item/user/amount/source vào log hệ thống.
    /// </summary>
    protected override Task HandleDomainEventAsync(
        EscrowRefundedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        // Bổ sung refund source để phân biệt hoàn tiền do dispute, timeout hay rule khác.
        _logger.LogInformation(
            "[DomainEvent] Escrow refunded. ItemId={ItemId}, UserId={UserId}, Amount={Amount}, Source={Source}",
            domainEvent.ItemId,
            domainEvent.UserId,
            domainEvent.AmountDiamond,
            domainEvent.RefundSource);

        return Task.CompletedTask;
    }
}

// Handler ghi log khi kết thúc billing cho phiên đọc bài AI.
public sealed class ReadingBillingCompletedNotificationHandler
    : IdempotentDomainEventNotificationHandler<ReadingBillingCompletedDomainEvent>
{
    private readonly ILogger<ReadingBillingCompletedNotificationHandler> _logger;

    /// <summary>
    /// Khởi tạo handler logging cho sự kiện hoàn tất billing phiên đọc bài.
    /// Luồng xử lý: nhận logger typed để ghi các trường billing quan trọng.
    /// </summary>
    public ReadingBillingCompletedNotificationHandler(
        ILogger<ReadingBillingCompletedNotificationHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ghi log kết quả cuối của billing đọc bài AI.
    /// Luồng xử lý: lấy domain event và ghi các trường trạng thái/chi phí/phản hồi refund.
    /// </summary>
    protected override Task HandleDomainEventAsync(
        ReadingBillingCompletedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        // Log session ref + final status để dễ truy vết luồng thu phí theo từng phiên AI cụ thể.
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
