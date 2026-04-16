using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class GachaPulledDomainEventHandler
{
    private async Task<PullExecutionOutcome> ExecutePullRollsAsync(
        PullExecutionContext context,
        IReadOnlyList<GachaPoolRewardRate> rates,
        CancellationToken cancellationToken)
    {
        var rewardLogs = new List<GachaPullRewardLog>(context.DomainEvent.Count);
        var wasPityReset = false;
        var wasPityTriggered = false;

        for (var index = 0; index < context.DomainEvent.Count; index++)
        {
            var pityCountAtRoll = checked(context.UserPity.PullCount + 1);
            var roll = SelectReward(rates, context.Pool, context.UserPity.PullCount);
            var isRareItemReward = IsRareItemReward(roll.SelectedRate);
            ApplyPityState(context.UserPity, isRareItemReward);

            var rewardLog = await ApplyRewardAndBuildLogAsync(
                context,
                roll,
                pityCountAtRoll,
                cancellationToken);
            rewardLogs.Add(rewardLog);

            wasPityTriggered = wasPityTriggered || roll.IsPityForced;
            wasPityReset = wasPityReset || isRareItemReward;
        }

        await _gachaPoolRepository.SaveUserPityAsync(context.UserPity, cancellationToken);

        context.Operation.MarkCompleted(context.UserPity.PullCount, context.Pool.HardPityCount, wasPityTriggered);
        context.DomainEvent.CurrentPityCount = context.UserPity.PullCount;
        context.DomainEvent.HardPityThreshold = context.Pool.HardPityCount;
        context.DomainEvent.WasPityTriggered = wasPityTriggered;

        return new PullExecutionOutcome(rewardLogs, wasPityReset);
    }

    private Task<GachaPullRewardLog> ApplyRewardAndBuildLogAsync(
        PullExecutionContext context,
        PullRollSelection roll,
        int pityCountAtRoll,
        CancellationToken cancellationToken)
    {
        return string.Equals(roll.SelectedRate.RewardKind, GachaRewardTypes.Currency, StringComparison.Ordinal)
            ? ApplyCurrencyRewardAsync(context, roll, pityCountAtRoll, cancellationToken)
            : ApplyItemRewardAsync(context, roll, pityCountAtRoll, cancellationToken);
    }

    private async Task<GachaPullRewardLog> ApplyCurrencyRewardAsync(
        PullExecutionContext context,
        PullRollSelection roll,
        int pityCountAtRoll,
        CancellationToken cancellationToken)
    {
        var currency = roll.SelectedRate.Currency
            ?? throw new BusinessRuleException(GachaErrorCodes.RewardResolutionFailed, "Currency reward is missing currency.");
        var amount = roll.SelectedRate.Amount
            ?? throw new BusinessRuleException(GachaErrorCodes.RewardResolutionFailed, "Currency reward is missing amount.");
        var totalAmount = checked(amount * roll.SelectedRate.QuantityGranted);

        await _walletRepository.CreditAsync(
            context.DomainEvent.UserId,
            currency,
            TransactionType.GachaReward,
            totalAmount,
            description: $"Gacha reward {context.Pool.Code}",
            idempotencyKey: $"gacha_pull_reward_{currency}_{context.Operation.Id}_{roll.SelectedRate.Id}",
            cancellationToken: cancellationToken);

        var moneyChanged = new MoneyChangeRequest(
            context.DomainEvent.UserId,
            currency,
            TransactionType.GachaReward,
            totalAmount,
            context.Operation.Id.ToString("N"));
        await PublishMoneyChangedAsync(moneyChanged, cancellationToken);

        return new GachaPullRewardLog(new GachaPullRewardLogCreateRequest(
            context.Operation.Id,
            context.DomainEvent.UserId,
            context.Pool.Id,
            context.Pool.Code,
            roll.SelectedRate.Id,
            roll.SelectedRate.RewardKind,
            roll.SelectedRate.Rarity,
            null,
            null,
            currency,
            totalAmount,
            roll.SelectedRate.QuantityGranted,
            roll.SelectedRate.IconUrl,
            roll.SelectedRate.NameVi,
            roll.SelectedRate.NameEn,
            roll.SelectedRate.NameZh,
            roll.IsPityForced,
            pityCountAtRoll,
            roll.RngSeed));
    }

    private async Task<GachaPullRewardLog> ApplyItemRewardAsync(
        PullExecutionContext context,
        PullRollSelection roll,
        int pityCountAtRoll,
        CancellationToken cancellationToken)
    {
        var itemDefinition = ResolveItemDefinition(context, roll);
        await _userItemRepository.GrantItemByCodeAsync(
            context.DomainEvent.UserId,
            itemDefinition.Code,
            roll.SelectedRate.QuantityGranted,
            cancellationToken);
        await PublishItemRewardEventsAsync(context, roll, itemDefinition, cancellationToken);

        return BuildItemRewardLog(context, roll, pityCountAtRoll, itemDefinition);
    }

    private static bool IsRareItemReward(GachaPoolRewardRate selectedRate)
    {
        return string.Equals(selectedRate.RewardKind, GachaRewardTypes.Item, StringComparison.Ordinal)
               && GachaRarity.IsAtLeastLegendary(selectedRate.Rarity);
    }

    private static void ApplyPityState(UserGachaPity userPity, bool isRareItemReward)
    {
        if (isRareItemReward)
        {
            userPity.Reset();
            return;
        }

        userPity.Increment();
    }

    private sealed record PullExecutionOutcome(
        IReadOnlyList<GachaPullRewardLog> RewardLogs,
        bool WasPityReset);
}
