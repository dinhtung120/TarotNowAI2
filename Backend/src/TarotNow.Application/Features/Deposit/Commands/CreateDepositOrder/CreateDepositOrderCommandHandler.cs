using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

// Handler tạo order theo Rule 0: chỉ publish domain event.
public class CreateDepositOrderCommandHandler : IRequestHandler<CreateDepositOrderCommand, CreateDepositOrderResponse>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler tạo đơn nạp.
    /// </summary>
    public CreateDepositOrderCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command tạo đơn nạp bằng cách publish domain event.
    /// </summary>
    public async Task<CreateDepositOrderResponse> Handle(
        CreateDepositOrderCommand request,
        CancellationToken cancellationToken)
    {
        var domainEvent = new DepositOrderCreateRequestedDomainEvent
        {
            UserId = request.UserId,
            PackageCode = request.PackageCode,
            IdempotencyKey = request.IdempotencyKey
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);

        return new CreateDepositOrderResponse
        {
            OrderId = domainEvent.OrderId,
            Status = domainEvent.Status,
            AmountVnd = domainEvent.AmountVnd,
            BaseDiamondAmount = domainEvent.BaseDiamondAmount,
            BonusGoldAmount = domainEvent.BonusGoldAmount,
            TotalDiamondAmount = domainEvent.TotalDiamondAmount,
            PayOsOrderCode = domainEvent.PayOsOrderCode,
            CheckoutUrl = domainEvent.CheckoutUrl,
            QrCode = domainEvent.QrCode,
            PaymentLinkId = domainEvent.PaymentLinkId,
            ExpiresAtUtc = domainEvent.ExpiresAtUtc
        };
    }
}
