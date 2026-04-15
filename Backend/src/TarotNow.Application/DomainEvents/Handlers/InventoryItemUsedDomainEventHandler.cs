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

    /// <summary>
    /// Khởi tạo handler ItemUsedDomainEvent.
    /// </summary>
    public ItemUsedDomainEventHandler(
        IItemDefinitionRepository itemDefinitionRepository,
        IUserItemRepository userItemRepository,
        IUserCollectionRepository userCollectionRepository,
        IInlineDomainEventDispatcher inlineDomainEventDispatcher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _itemDefinitionRepository = itemDefinitionRepository;
        _userItemRepository = userItemRepository;
        _userCollectionRepository = userCollectionRepository;
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ItemUsedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var definition = await GetActiveDefinitionAsync(domainEvent.ItemCode, cancellationToken);
        var userItem = await GetOwnedItemAsync(domainEvent.UserId, definition.Id, cancellationToken);
        EnsureQuantityIfConsumable(definition, userItem);
        var targetCardId = await ResolveTargetCardIdAsync(domainEvent, definition.Type, cancellationToken);

        var registered = await _userItemRepository.TryRegisterUseOperationAsync(
            new InventoryItemUseOperation(
                domainEvent.UserId,
                domainEvent.IdempotencyKey,
                definition.Code,
                targetCardId),
            cancellationToken);
        if (registered == false)
        {
            return;
        }

        await ConsumeQuantityIfNeededAsync(definition, userItem, cancellationToken);
        await DispatchItemEffectAsync(domainEvent.UserId, definition, targetCardId, cancellationToken);
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

    private async Task<UserItem> GetOwnedItemAsync(Guid userId, Guid itemDefinitionId, CancellationToken cancellationToken)
    {
        var userItem = await _userItemRepository.GetByUserAndItemDefinitionIdAsync(userId, itemDefinitionId, cancellationToken);
        if (userItem is null)
        {
            throw new BusinessRuleException(InventoryErrorCodes.ItemNotOwned, "User does not own this item.");
        }

        return userItem;
    }

    private static void EnsureQuantityIfConsumable(ItemDefinition definition, UserItem userItem)
    {
        if (definition.IsConsumable && userItem.Quantity <= 0)
        {
            throw new BusinessRuleException(InventoryErrorCodes.ItemOutOfStock, "Item quantity is not enough.");
        }
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

        if (domainEvent.TargetCardId is null || domainEvent.TargetCardId <= 0)
        {
            throw new BusinessRuleException(
                InventoryErrorCodes.TargetCardRequired,
                "This item requires a target card.");
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

    private async Task ConsumeQuantityIfNeededAsync(
        ItemDefinition definition,
        UserItem userItem,
        CancellationToken cancellationToken)
    {
        if (definition.IsConsumable == false)
        {
            return;
        }

        userItem.DecreaseQuantity(MinimumEffectValue);
        await _userItemRepository.UpdateAsync(userItem, cancellationToken);
    }

}
