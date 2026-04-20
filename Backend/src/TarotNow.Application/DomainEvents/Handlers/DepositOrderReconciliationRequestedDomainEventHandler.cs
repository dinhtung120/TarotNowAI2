using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler reconcile đơn nạp pending bằng trạng thái trực tiếp từ PayOS.
/// </summary>
public sealed class DepositOrderReconciliationRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DepositOrderReconciliationRequestedDomainEvent>
{
    private const string PayOsStatusPaid = "PAID";
    private const string PayOsStatusCancelled = "CANCELLED";
    private const string PayOsStatusExpired = "EXPIRED";

    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IPayOsGateway _payOsGateway;
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler reconcile đơn nạp.
    /// </summary>
    public DepositOrderReconciliationRequestedDomainEventHandler(
        IDepositOrderRepository depositOrderRepository,
        IPayOsGateway payOsGateway,
        IInlineDomainEventDispatcher inlineDomainEventDispatcher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _depositOrderRepository = depositOrderRepository;
        _payOsGateway = payOsGateway;
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        DepositOrderReconciliationRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var order = await _depositOrderRepository.GetByIdForUpdateAsync(domainEvent.OrderId, cancellationToken);
        if (order == null || order.UserId != domainEvent.UserId)
        {
            return;
        }

        if (order.Status == DepositOrderStatus.Success && order.WalletGrantedAtUtc.HasValue)
        {
            domainEvent.Handled = true;
            return;
        }

        var payOsPaymentInfo = await _payOsGateway.GetPaymentLinkInformationAsync(order.PayOsOrderCode, cancellationToken);
        ValidateAmountInvariant(order, payOsPaymentInfo.Amount);

        if (IsPaidStatus(payOsPaymentInfo.PaymentStatus))
        {
            await ProcessSuccessStateAsync(domainEvent, order, payOsPaymentInfo, cancellationToken);
            return;
        }

        if (IsFailureStatus(payOsPaymentInfo.PaymentStatus))
        {
            await ProcessFailureStateAsync(domainEvent, order, payOsPaymentInfo, cancellationToken);
            return;
        }

        domainEvent.Handled = true;
    }

    private async Task ProcessSuccessStateAsync(
        DepositOrderReconciliationRequestedDomainEvent domainEvent,
        DepositOrder order,
        PayOsPaymentLinkInformation paymentInfo,
        CancellationToken cancellationToken)
    {
        var reference = ResolveReference(paymentInfo, order);
        if (order.Status != DepositOrderStatus.Success)
        {
            order.MarkAsSuccess(reference, paymentInfo.LatestTransactionAtUtc);
            await _depositOrderRepository.UpdateAsync(order, cancellationToken);
        }

        await _inlineDomainEventDispatcher.PublishAsync(
            new DepositPaymentSucceededDomainEvent
            {
                OrderId = order.Id,
                UserId = order.UserId,
                DiamondAmount = order.DiamondAmount,
                BonusGoldAmount = order.BonusGoldAmount,
                ReferenceId = reference
            },
            cancellationToken);

        domainEvent.Handled = true;
    }

    private async Task ProcessFailureStateAsync(
        DepositOrderReconciliationRequestedDomainEvent domainEvent,
        DepositOrder order,
        PayOsPaymentLinkInformation paymentInfo,
        CancellationToken cancellationToken)
    {
        if (order.Status == DepositOrderStatus.Pending)
        {
            order.MarkAsFailed(
                paymentInfo.FailureReason,
                paymentInfo.LatestReference,
                paymentInfo.LatestTransactionAtUtc);
            await _depositOrderRepository.UpdateAsync(order, cancellationToken);
        }

        domainEvent.Handled = true;
    }

    private static bool IsPaidStatus(string status)
    {
        return string.Equals(status, PayOsStatusPaid, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsFailureStatus(string status)
    {
        return string.Equals(status, PayOsStatusCancelled, StringComparison.OrdinalIgnoreCase)
               || string.Equals(status, PayOsStatusExpired, StringComparison.OrdinalIgnoreCase);
    }

    private static string ResolveReference(PayOsPaymentLinkInformation paymentInfo, DepositOrder order)
    {
        if (string.IsNullOrWhiteSpace(paymentInfo.LatestReference) == false)
        {
            return paymentInfo.LatestReference.Trim();
        }

        if (string.IsNullOrWhiteSpace(order.TransactionId) == false)
        {
            return order.TransactionId.Trim();
        }

        return order.PayOsOrderCode.ToString();
    }

    private static void ValidateAmountInvariant(DepositOrder order, long payOsAmount)
    {
        if (order.AmountVnd == payOsAmount)
        {
            return;
        }

        throw new BadRequestException("PayOS payment amount does not match deposit order amount.");
    }
}
