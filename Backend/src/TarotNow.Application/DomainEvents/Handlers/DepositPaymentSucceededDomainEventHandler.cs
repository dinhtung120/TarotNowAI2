using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler cấp ví sau khi thanh toán nạp thành công.
/// </summary>
public sealed class DepositPaymentSucceededDomainEventHandler
    : IdempotentDomainEventNotificationHandler<DepositPaymentSucceededDomainEvent>
{
    private const string DepositReferenceSource = "DepositOrder";

    private readonly IDepositOrderRepository _depositOrderRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler cấp ví cho lệnh nạp.
    /// </summary>
    public DepositPaymentSucceededDomainEventHandler(
        IDepositOrderRepository depositOrderRepository,
        IWalletRepository walletRepository,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _depositOrderRepository = depositOrderRepository;
        _walletRepository = walletRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        DepositPaymentSucceededDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var order = await _depositOrderRepository.GetByIdForUpdateAsync(domainEvent.OrderId, cancellationToken);
        if (order == null || order.WalletGrantedAtUtc.HasValue)
        {
            return;
        }

        await CreditDiamondAsync(domainEvent, cancellationToken);
        await CreditBonusGoldAsync(domainEvent, cancellationToken);

        order.MarkWalletGranted();
        await _depositOrderRepository.UpdateAsync(order, cancellationToken);
    }

    private async Task CreditDiamondAsync(
        DepositPaymentSucceededDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        if (domainEvent.DiamondAmount <= 0)
        {
            return;
        }

        var operationResult = await _walletRepository.CreditWithResultAsync(
            userId: domainEvent.UserId,
            currency: CurrencyType.Diamond,
            type: TransactionType.Deposit,
            amount: domainEvent.DiamondAmount,
            referenceSource: DepositReferenceSource,
            referenceId: domainEvent.OrderId.ToString(),
            description: $"Top-up deposit order {domainEvent.OrderId}",
            metadataJson: null,
            idempotencyKey: BuildDiamondCreditIdempotencyKey(domainEvent.OrderId),
            cancellationToken: cancellationToken);
        if (operationResult.Executed == false)
        {
            return;
        }

        await PublishMoneyChangedAsync(
            new MoneyChangedDomainEvent
            {
                UserId = domainEvent.UserId,
                Currency = CurrencyType.Diamond,
                ChangeType = TransactionType.Deposit,
                DeltaAmount = domainEvent.DiamondAmount,
                ReferenceId = domainEvent.ReferenceId
            },
            cancellationToken);
    }

    private async Task CreditBonusGoldAsync(
        DepositPaymentSucceededDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        if (domainEvent.BonusGoldAmount <= 0)
        {
            return;
        }

        var operationResult = await _walletRepository.CreditWithResultAsync(
            userId: domainEvent.UserId,
            currency: CurrencyType.Gold,
            type: TransactionType.DepositBonus,
            amount: domainEvent.BonusGoldAmount,
            referenceSource: DepositReferenceSource,
            referenceId: domainEvent.OrderId.ToString(),
            description: $"Top-up bonus gold from order {domainEvent.OrderId}",
            metadataJson: null,
            idempotencyKey: BuildGoldBonusIdempotencyKey(domainEvent.OrderId),
            cancellationToken: cancellationToken);
        if (operationResult.Executed == false)
        {
            return;
        }

        await PublishMoneyChangedAsync(
            new MoneyChangedDomainEvent
            {
                UserId = domainEvent.UserId,
                Currency = CurrencyType.Gold,
                ChangeType = TransactionType.DepositBonus,
                DeltaAmount = domainEvent.BonusGoldAmount,
                ReferenceId = domainEvent.ReferenceId
            },
            cancellationToken);
    }

    private Task PublishMoneyChangedAsync(
        MoneyChangedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        return _domainEventPublisher.PublishAsync(domainEvent, cancellationToken);
    }

    private static string BuildDiamondCreditIdempotencyKey(Guid orderId)
        => $"deposit:{orderId}:diamond";

    private static string BuildGoldBonusIdempotencyKey(Guid orderId)
        => $"deposit:{orderId}:bonus_gold";
}
