using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effect cho item Lucky Star: grant title hoặc thưởng vàng nếu đã sở hữu title.
/// </summary>
public sealed class LuckyStarTitleUsedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<LuckyStarTitleUsedDomainEvent>
{
    private readonly ITitleRepository _titleRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo handler Lucky Star.
    /// </summary>
    public LuckyStarTitleUsedDomainEventHandler(
        ITitleRepository titleRepository,
        IWalletRepository walletRepository,
        IDomainEventPublisher domainEventPublisher,
        ISystemConfigSettings systemConfigSettings,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _titleRepository = titleRepository;
        _walletRepository = walletRepository;
        _domainEventPublisher = domainEventPublisher;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        LuckyStarTitleUsedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var luckyStarTitleCode = InventoryBusinessConstants.LuckyStarTitleCode;
        var alreadyOwnsTitle = await _titleRepository.OwnsTitleAsync(
            domainEvent.UserId,
            luckyStarTitleCode,
            cancellationToken);
        if (alreadyOwnsTitle == false)
        {
            await _domainEventPublisher.PublishAsync(
                new TitleGrantedDomainEvent
                {
                    UserId = domainEvent.UserId,
                    TitleCode = luckyStarTitleCode,
                    Source = domainEvent.SourceItemCode,
                },
                cancellationToken);
            return;
        }

        var amount = _systemConfigSettings.InventoryLuckyStarOwnedTitleGoldReward;
        var stableIdempotencyKey = string.IsNullOrWhiteSpace(domainEvent.SourceIdempotencyKey)
            ? $"inventory_lucky_star_reward_{domainEvent.UserId:N}_{domainEvent.SourceItemCode}_{domainEvent.OccurredAtUtc:yyyyMMddHHmmss}"
            : $"inventory_lucky_star_reward_{domainEvent.SourceIdempotencyKey}";
        var operationResult = await _walletRepository.CreditWithResultAsync(
            userId: domainEvent.UserId,
            currency: CurrencyType.Gold,
            type: TransactionType.InventoryReward,
            amount: amount,
            referenceSource: "Inventory",
            referenceId: domainEvent.SourceItemCode,
            description: "Lucky Star bonus because title was already owned.",
            metadataJson: null,
            idempotencyKey: stableIdempotencyKey,
            cancellationToken: cancellationToken);
        if (operationResult.Executed == false)
        {
            return;
        }

        await _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = domainEvent.UserId,
                Currency = CurrencyType.Gold,
                ChangeType = TransactionType.InventoryReward,
                DeltaAmount = amount,
                ReferenceId = domainEvent.SourceItemCode,
            },
            cancellationToken);
    }
}
