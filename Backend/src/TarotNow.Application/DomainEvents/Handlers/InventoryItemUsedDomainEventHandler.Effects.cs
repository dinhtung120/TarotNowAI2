using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events.Inventory;
using TarotNow.Domain.ValueObjects;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class ItemUsedDomainEventHandler
{
    private async Task<InventoryItemEffectSummary?> DispatchItemEffectAsync(
        Guid userId,
        ItemDefinition definition,
        int? targetCardId,
        CancellationToken cancellationToken)
    {
        if (string.Equals(definition.Type, ItemType.CardEnhancer, StringComparison.OrdinalIgnoreCase))
        {
            return await HandleCardEnhancerAsync(userId, definition, targetCardId, cancellationToken);
        }

        if (string.Equals(definition.Type, ItemType.ReadingBooster, StringComparison.OrdinalIgnoreCase))
        {
            return await PublishFreeDrawGrantedAsync(userId, definition, cancellationToken);
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

            return new InventoryItemEffectSummary
            {
                EffectType = definition.Type,
                RolledValue = 0m,
                CardId = null,
                Before = null,
                After = null,
            };
        }

        throw new BusinessRuleException(InventoryErrorCodes.UnsupportedItemType, "Item type is not supported.");
    }

    private async Task<InventoryItemEffectSummary> HandleCardEnhancerAsync(
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

        return new InventoryItemEffectSummary
        {
            EffectType = enhancementType,
            RolledValue = applyResult.RolledValue,
            CardId = targetCardId,
            Before = MapCardSnapshot(applyResult.BeforeStats),
            After = MapCardSnapshot(applyResult.AfterStats),
        };
    }

    private async Task<InventoryItemEffectSummary> PublishFreeDrawGrantedAsync(
        Guid userId,
        ItemDefinition definition,
        CancellationToken cancellationToken)
    {
        var grantedCount = Math.Max(MinimumEffectValue, definition.EffectValue);
        var spreadCardCount = ResolveFreeDrawSpreadCardCount(definition.Code);

        await _inlineDomainEventDispatcher.PublishAsync(
            new FreeDrawGrantedDomainEvent
            {
                UserId = userId,
                GrantedCount = grantedCount,
                SpreadCardCount = spreadCardCount,
                SourceItemCode = definition.Code,
            },
            cancellationToken);

        return new InventoryItemEffectSummary
        {
            EffectType = EnhancementType.FreeDraw,
            RolledValue = grantedCount,
            CardId = null,
            Before = null,
            After = null,
        };
    }

    private static InventoryCardStatSnapshot MapCardSnapshot(CardEnhancementStatSnapshot snapshot)
    {
        return new InventoryCardStatSnapshot
        {
            Level = snapshot.Level,
            CurrentExp = snapshot.CurrentExp,
            ExpToNextLevel = snapshot.ExpToNextLevel,
            BaseAtk = snapshot.BaseAtk,
            BaseDef = snapshot.BaseDef,
            BonusAtkPercent = snapshot.BonusAtkPercent,
            BonusDefPercent = snapshot.BonusDefPercent,
            TotalAtk = snapshot.TotalAtk,
            TotalDef = snapshot.TotalDef,
        };
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
