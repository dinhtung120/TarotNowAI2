using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Deposit.Commands.ReconcileMyDepositOrder;

/// <summary>
/// Handler command reconcile đơn nạp theo Rule 0: chỉ publish domain event.
/// </summary>
public sealed class ReconcileMyDepositOrderCommandHandler : IRequestHandler<ReconcileMyDepositOrderCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler reconcile đơn nạp.
    /// </summary>
    public ReconcileMyDepositOrderCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Publish domain event reconcile đơn nạp và trả kết quả xử lý.
    /// </summary>
    public async Task<bool> Handle(ReconcileMyDepositOrderCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new DepositOrderReconciliationRequestedDomainEvent
        {
            UserId = request.UserId,
            OrderId = request.OrderId
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Handled;
    }
}
