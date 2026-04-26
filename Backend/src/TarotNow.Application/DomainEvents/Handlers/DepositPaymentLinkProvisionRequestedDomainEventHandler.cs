using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler tạo payment link PayOS bất đồng bộ từ outbox sau khi đơn nạp đã được commit.
/// </summary>
public sealed class DepositPaymentLinkProvisionRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DepositPaymentLinkProvisionRequestedDomainEvent>
{
    private const string DescriptionPrefix = "TOPUP";

    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IPayOsGateway _payOsGateway;
    private readonly IDepositPayOsSettings _depositPayOsSettings;

    public DepositPaymentLinkProvisionRequestedDomainEventHandler(
        IDepositOrderRepository depositOrderRepository,
        IPayOsGateway payOsGateway,
        IDepositPayOsSettings depositPayOsSettings,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _depositOrderRepository = depositOrderRepository;
        _payOsGateway = payOsGateway;
        _depositPayOsSettings = depositPayOsSettings;
    }

    protected override async Task HandleDomainEventAsync(
        DepositPaymentLinkProvisionRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var order = await _depositOrderRepository.GetByIdForUpdateAsync(domainEvent.OrderId, cancellationToken);
        if (order is null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(order.PayOsPaymentLinkId)
            && !string.IsNullOrWhiteSpace(order.CheckoutUrl)
            && !string.IsNullOrWhiteSpace(order.QrCode))
        {
            return;
        }

        try
        {
            var paymentLink = await CreatePaymentLinkAsync(order, cancellationToken);
            order.MarkPaymentLinkReady(
                paymentLink.PaymentLinkId,
                paymentLink.CheckoutUrl,
                paymentLink.QrCode,
                paymentLink.ExpiresAtUtc,
                DateTime.UtcNow);
            await _depositOrderRepository.UpdateAsync(order, cancellationToken);
        }
        catch (Exception exception)
        {
            order.MarkPaymentLinkProvisionFailed(exception.Message, DateTime.UtcNow);
            await _depositOrderRepository.UpdateAsync(order, cancellationToken);
            throw;
        }
    }

    private Task<PayOsCreatePaymentLinkResult> CreatePaymentLinkAsync(
        Domain.Entities.DepositOrder order,
        CancellationToken cancellationToken)
    {
        var expiredAtUnix = DateTimeOffset.UtcNow
            .AddMinutes(_depositPayOsSettings.LinkExpiryMinutes)
            .ToUnixTimeSeconds();

        return _payOsGateway.CreatePaymentLinkAsync(
            new PayOsCreatePaymentLinkRequest
            {
                OrderCode = order.PayOsOrderCode,
                Amount = order.AmountVnd,
                Description = BuildPaymentDescription(order.PayOsOrderCode),
                CancelUrl = _depositPayOsSettings.CancelUrl,
                ReturnUrl = _depositPayOsSettings.ReturnUrl,
                ExpiredAtUnix = expiredAtUnix,
                Items =
                [
                    new PayOsPaymentItem
                    {
                        Name = $"TarotNow {order.PackageCode}",
                        Quantity = 1,
                        Price = order.AmountVnd
                    }
                ]
            },
            cancellationToken);
    }

    private static string BuildPaymentDescription(long orderCode)
    {
        return $"{DescriptionPrefix} {orderCode}";
    }
}
