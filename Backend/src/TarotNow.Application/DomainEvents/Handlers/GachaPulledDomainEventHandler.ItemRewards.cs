using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events.Gacha;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class GachaPulledDomainEventHandler
{
    private ItemDefinition ResolveItemDefinition(PullExecutionContext context, PullRollSelection roll)
    {
        var itemDefinitionId = roll.SelectedRate.ItemDefinitionId
            ?? throw new BusinessRuleException(GachaErrorCodes.RewardResolutionFailed, "Item reward is missing item definition id.");
        if (!context.ItemDefinitionMap.TryGetValue(itemDefinitionId, out var itemDefinition))
        {
            throw new BusinessRuleException(GachaErrorCodes.RewardResolutionFailed, "Item reward definition was not resolved.");
        }

        return itemDefinition;
    }

    private async Task PublishItemRewardEventsAsync(
        PullExecutionContext context,
        PullRollSelection roll,
        ItemDefinition itemDefinition,
        CancellationToken cancellationToken)
    {
        await _inlineDomainEventDispatcher.PublishAsync(
            new ItemGrantedFromGachaDomainEvent
            {
                UserId = context.DomainEvent.UserId,
                ItemDefinitionId = itemDefinition.Id,
                ItemCode = itemDefinition.Code,
                QuantityGranted = roll.SelectedRate.QuantityGranted,
                PoolCode = context.Pool.Code,
                PullOperationId = context.Operation.Id,
            },
            cancellationToken);
        if (roll.IsPityForced)
        {
            await _inlineDomainEventDispatcher.PublishAsync(
                new PityTriggeredDomainEvent
                {
                    UserId = context.DomainEvent.UserId,
                    PoolCode = context.Pool.Code,
                    PullOperationId = context.Operation.Id,
                    RarityForced = roll.SelectedRate.Rarity,
                },
                cancellationToken);
        }
    }

    private static GachaPullRewardLog BuildItemRewardLog(
        PullExecutionContext context,
        PullRollSelection roll,
        int pityCountAtRoll,
        ItemDefinition itemDefinition)
    {
        return new GachaPullRewardLog(new GachaPullRewardLogCreateRequest(
            context.Operation.Id,
            context.DomainEvent.UserId,
            context.Pool.Id,
            context.Pool.Code,
            roll.SelectedRate.Id,
            roll.SelectedRate.RewardKind,
            roll.SelectedRate.Rarity,
            itemDefinition.Code,
            itemDefinition.Id,
            null,
            null,
            roll.SelectedRate.QuantityGranted,
            roll.SelectedRate.IconUrl ?? itemDefinition.IconUrl,
            roll.SelectedRate.NameVi,
            roll.SelectedRate.NameEn,
            roll.SelectedRate.NameZh,
            roll.IsPityForced,
            pityCountAtRoll,
            roll.RngSeed));
    }
}

