using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events.Inventory;
using TarotNow.Domain.ValueObjects;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class ItemUsedDomainEventHandler
{
    private async Task DispatchItemEffectAsync(
        Guid userId,
        ItemDefinition definition,
        int? targetCardId,
        CancellationToken cancellationToken)
    {
        if (string.Equals(definition.Type, ItemType.CardEnhancer, StringComparison.OrdinalIgnoreCase))
        {
            await HandleCardEnhancerAsync(userId, definition, targetCardId, cancellationToken);
            return;
        }

        if (string.Equals(definition.Type, ItemType.ReadingBooster, StringComparison.OrdinalIgnoreCase))
        {
            await PublishFreeDrawGrantedAsync(userId, definition, cancellationToken);
            return;
        }

        if (string.Equals(definition.Type, ItemType.ConsumableSpecial, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessRuleException(InventoryErrorCodes.UnsupportedItemType, "Consumable special item is not supported.");
        }

        if (string.Equals(definition.Type, ItemType.RareTitle, StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(definition.Code, InventoryItemCodes.RareTitleLuckyStar, StringComparison.OrdinalIgnoreCase) == false)
            {
                throw new BusinessRuleException(InventoryErrorCodes.UnsupportedItemType, "Rare title item is not supported.");
            }

            await _inlineDomainEventDispatcher.PublishAsync(
                new LuckyStarTitleUsedDomainEvent
                {
                    UserId = userId,
                    SourceItemCode = definition.Code,
                },
                cancellationToken);
            return;
        }

        throw new BusinessRuleException(InventoryErrorCodes.UnsupportedItemType, "Item type is not supported.");
    }

    private async Task HandleCardEnhancerAsync(
        Guid userId,
        ItemDefinition definition,
        int? targetCardId,
        CancellationToken cancellationToken)
    {
        if (targetCardId is null)
        {
            throw new BusinessRuleException(InventoryErrorCodes.TargetCardRequired, "This item requires a target card.");
        }

        var enhancementType = definition.EnhancementType?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(enhancementType))
        {
            throw new BusinessRuleException(InventoryErrorCodes.UnsupportedItemType, "Enhancement type is not supported.");
        }

        var applyResult = await _userCollectionRepository.ApplyEnhancementAsync(
            new CardEnhancementApplyRequest
            {
                UserId = userId,
                CardId = targetCardId.Value,
                EnhancementType = enhancementType,
                EffectValue = definition.EffectValue,
                SuccessRatePercent = definition.SuccessRatePercent,
            },
            cancellationToken);

        await _inlineDomainEventDispatcher.PublishAsync(
            new CardEnhancedDomainEvent
            {
                UserId = userId,
                CardId = targetCardId.Value,
                EnhancementType = enhancementType,
                ExpDelta = applyResult.ExpDelta,
                AttackDelta = applyResult.AttackDelta,
                DefenseDelta = applyResult.DefenseDelta,
                UpgradeSucceeded = applyResult.LevelUpgraded,
                SourceItemCode = definition.Code,
            },
            cancellationToken);
    }

    private Task PublishFreeDrawGrantedAsync(
        Guid userId,
        ItemDefinition definition,
        CancellationToken cancellationToken)
    {
        return _inlineDomainEventDispatcher.PublishAsync(
            new FreeDrawGrantedDomainEvent
            {
                UserId = userId,
                GrantedCount = Math.Max(MinimumEffectValue, definition.EffectValue),
                SpreadCardCount = ResolveFreeDrawSpreadCardCount(definition.Code),
                SourceItemCode = definition.Code,
            },
            cancellationToken);
    }

    private static int ResolveFreeDrawSpreadCardCount(string itemCode)
    {
        var normalizedItemCode = itemCode.Trim().ToLowerInvariant();
        var spreadCardCount = InventoryBusinessConstants.ResolveTicketSpreadCardCount(normalizedItemCode);
        if (spreadCardCount is not null)
        {
            return spreadCardCount.Value;
        }

        throw new BusinessRuleException(InventoryErrorCodes.UnsupportedItemType, "Free draw ticket item is not supported.");
    }
}
