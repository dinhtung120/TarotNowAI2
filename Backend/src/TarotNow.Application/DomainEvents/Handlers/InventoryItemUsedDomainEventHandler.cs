using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events.Inventory;
using TarotNow.Domain.ValueObjects;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effects chính khi người dùng dùng item inventory.
/// </summary>
public sealed partial class ItemUsedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ItemUsedDomainEvent>
{
    private const int MinimumEffectValue = 1;

    private readonly IItemDefinitionRepository _itemDefinitionRepository;
    private readonly IUserItemRepository _userItemRepository;
    private readonly IUserCollectionRepository _userCollectionRepository;
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;
    private readonly IFreeDrawCreditRepository _freeDrawCreditRepository;

    /// <summary>
    /// Khởi tạo handler ItemUsedDomainEvent.
    /// </summary>
    public ItemUsedDomainEventHandler(
        IItemDefinitionRepository itemDefinitionRepository,
        IUserItemRepository userItemRepository,
        IUserCollectionRepository userCollectionRepository,
        IInlineDomainEventDispatcher inlineDomainEventDispatcher,
        IFreeDrawCreditRepository freeDrawCreditRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _itemDefinitionRepository = itemDefinitionRepository;
        _userItemRepository = userItemRepository;
        _userCollectionRepository = userCollectionRepository;
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
        _freeDrawCreditRepository = freeDrawCreditRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ItemUsedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var definition = await GetActiveDefinitionAsync(domainEvent.ItemCode, cancellationToken);
        var targetCardId = await ResolveTargetCardIdAsync(domainEvent, definition.Type, cancellationToken);

        var quantityToUse = ResolveQuantityToUse(domainEvent.Quantity, definition.IsConsumable);

        var consumeResult = await _userItemRepository.TryConsumeWithIdempotencyAsync(
            new InventoryItemConsumeRequest
            {
                UserId = domainEvent.UserId,
                ItemDefinitionId = definition.Id,
                ItemCode = definition.Code,
                TargetCardId = targetCardId,
                IdempotencyKey = domainEvent.IdempotencyKey,
                IsConsumable = definition.IsConsumable,
                ConsumeQuantity = quantityToUse,
            },
            cancellationToken);
        if (consumeResult == InventoryItemConsumeResult.AlreadyProcessed)
        {
            domainEvent.IsIdempotentReplay = true;
            return;
        }

        EnsureConsumeSucceeded(consumeResult);

        // Áp dụng hiệu ứng lặp lại theo số lượng đã chọn.
        var summaries = new List<InventoryItemEffectSummary>();
        for (int i = 0; i < quantityToUse; i++)
        {
            var effectContext = new ItemEffectDispatchContext(
                domainEvent.UserId,
                definition,
                targetCardId,
                domainEvent.IdempotencyKey,
                i);
            var summary = await DispatchItemEffectAsync(
                effectContext,
                cancellationToken);
            if (summary != null)
            {
                summaries.Add(summary);
            }
        }
        domainEvent.EffectSummaries = summaries;
    }

    private static int ResolveQuantityToUse(int requestedQuantity, bool isConsumable)
    {
        var normalized = Math.Clamp(requestedQuantity, 1, 10);
        if (isConsumable)
        {
            return normalized;
        }

        if (normalized > 1)
        {
            throw new BusinessRuleException(
                InventoryErrorCodes.InvalidQuantity,
                "Non-consumable item can only be used with quantity = 1.");
        }

        return 1;
    }

    private async Task<ItemDefinition> GetActiveDefinitionAsync(string itemCode, CancellationToken cancellationToken)
    {
        var definition = await _itemDefinitionRepository.GetByCodeAsync(itemCode, cancellationToken);
        if (definition is null || definition.IsActive == false)
        {
            throw new BusinessRuleException(InventoryErrorCodes.ItemNotFound, "Inventory item definition was not found.");
        }

        return definition;
    }

    private static void EnsureConsumeSucceeded(InventoryItemConsumeResult consumeResult)
    {
        if (consumeResult == InventoryItemConsumeResult.Consumed)
        {
            return;
        }

        if (consumeResult == InventoryItemConsumeResult.ItemNotOwned)
        {
            throw new BusinessRuleException(InventoryErrorCodes.ItemNotOwned, "User does not own this item.");
        }

        if (consumeResult == InventoryItemConsumeResult.OutOfStock)
        {
            throw new BusinessRuleException(InventoryErrorCodes.ItemOutOfStock, "Item quantity is not enough.");
        }

        throw new BusinessRuleException(InventoryErrorCodes.UnsupportedItemType, "Inventory consume result is unsupported.");
    }

    private async Task<int?> ResolveTargetCardIdAsync(
        ItemUsedDomainEvent domainEvent,
        string itemType,
        CancellationToken cancellationToken)
    {
        if (string.Equals(itemType, ItemType.CardEnhancer, StringComparison.OrdinalIgnoreCase) == false)
        {
            return null;
        }

        if (domainEvent.TargetCardId is null || domainEvent.TargetCardId < 0)
        {
            throw new BusinessRuleException(
                InventoryErrorCodes.TargetCardRequired,
                "This item requires a valid target card.");
        }

        var cardOwned = await _userCollectionRepository.ExistsAsync(
            domainEvent.UserId,
            domainEvent.TargetCardId.Value,
            cancellationToken);
        if (cardOwned == false)
        {
            throw new BusinessRuleException(
                InventoryErrorCodes.TargetCardNotOwned,
                "Target card is not owned by user.");
        }

        return domainEvent.TargetCardId.Value;
    }

}
