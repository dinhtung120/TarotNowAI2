using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler write-side xử lý webhook nạp tiền từ PayOS.
/// </summary>
public sealed class DepositWebhookReceivedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DepositWebhookReceivedDomainEvent>
{
    private readonly IPayOsGateway _payOsGateway;
    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler xử lý webhook nạp.
    /// </summary>
    public DepositWebhookReceivedDomainEventHandler(
        IPayOsGateway payOsGateway,
        IDepositOrderRepository depositOrderRepository,
        IInlineDomainEventDispatcher inlineDomainEventDispatcher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _payOsGateway = payOsGateway;
        _depositOrderRepository = depositOrderRepository;
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        DepositWebhookReceivedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var verifiedData = await _payOsGateway.VerifyWebhookAsync(domainEvent.RawPayload, cancellationToken);
        var order = await _depositOrderRepository.GetByPayOsOrderCodeForUpdateAsync(
            verifiedData.OrderCode,
            cancellationToken);

        if (order == null)
        {
            domainEvent.Handled = true;
            return;
        }

        ValidateWebhookAmount(order, verifiedData.Amount);
        if (await TryHandleAlreadyProcessedOrderAsync(domainEvent, order, verifiedData))
        {
            return;
        }

        if (!verifiedData.IsSuccess)
        {
            await HandleFailedWebhookAsync(domainEvent, order, verifiedData, cancellationToken);
            return;
        }

        await HandleSuccessfulWebhookAsync(domainEvent, order, verifiedData, cancellationToken);
    }

    private static Task<bool> TryHandleAlreadyProcessedOrderAsync(
        DepositWebhookReceivedDomainEvent domainEvent,
        DepositOrder order,
        PayOsVerifiedWebhookData verifiedData)
    {
        if (order.Status != DepositOrderStatus.Success)
        {
            return Task.FromResult(false);
        }

        if (!verifiedData.IsSuccess)
        {
            domainEvent.Handled = true;
            return Task.FromResult(true);
        }

        if (!order.WalletGrantedAtUtc.HasValue)
        {
            return Task.FromResult(false);
        }

        domainEvent.Handled = true;
        return Task.FromResult(true);
    }

    private async Task HandleFailedWebhookAsync(
        DepositWebhookReceivedDomainEvent domainEvent,
        DepositOrder order,
        PayOsVerifiedWebhookData verifiedData,
        CancellationToken cancellationToken)
    {
        order.MarkAsFailed(verifiedData.FailureReason, verifiedData.Reference, verifiedData.TransactionAtUtc);
        await _depositOrderRepository.UpdateAsync(order, cancellationToken);
        domainEvent.Handled = true;
    }

    private async Task HandleSuccessfulWebhookAsync(
        DepositWebhookReceivedDomainEvent domainEvent,
        DepositOrder order,
        PayOsVerifiedWebhookData verifiedData,
        CancellationToken cancellationToken)
    {
        order.MarkAsSuccess(verifiedData.Reference, verifiedData.TransactionAtUtc);
        await _depositOrderRepository.UpdateAsync(order, cancellationToken);

        await _inlineDomainEventDispatcher.PublishAsync(
            new DepositPaymentSucceededDomainEvent
            {
                OrderId = order.Id,
                UserId = order.UserId,
                DiamondAmount = order.DiamondAmount,
                BonusGoldAmount = order.BonusGoldAmount,
                ReferenceId = verifiedData.Reference
            },
            cancellationToken);

        domainEvent.Handled = true;
    }

    private static void ValidateWebhookAmount(DepositOrder order, long webhookAmount)
    {
        if (order.AmountVnd != webhookAmount)
        {
            throw new BadRequestException("Webhook amount does not match order amount.");
        }
    }
}
