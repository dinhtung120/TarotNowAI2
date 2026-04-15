using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed partial class GachaPulledDomainEventHandler
{
    private PullRollSelection SelectReward(
        IReadOnlyList<GachaPoolRewardRate> rates,
        GachaPool pool,
        int currentPityCount)
    {
        var isPityForced = pool.PityEnabled
                           && pool.HardPityCount > 0
                           && currentPityCount >= pool.HardPityCount - 1;
        var candidateRates = isPityForced
            ? rates
                .Where(x =>
                    string.Equals(x.RewardKind, GachaRewardTypes.Item, StringComparison.Ordinal)
                    && GachaRarity.IsAtLeastEpic(x.Rarity))
                .ToList()
            : rates.ToList();

        if (candidateRates.Count == 0)
        {
            throw new BusinessRuleException(
                GachaErrorCodes.InvalidPoolConfiguration,
                "Hard pity is enabled but pool has no item reward with rarity Epic or higher.");
        }

        if (candidateRates.Count == 1)
        {
            return new PullRollSelection(candidateRates[0], isPityForced, null);
        }

        var weighted = candidateRates.Select(x => new WeightedItem
        {
            ItemId = x.Id,
            WeightBasisPoints = x.ProbabilityBasisPoints,
        });
        var rng = _rngService.WeightedSelect(weighted);
        var selectedRate = candidateRates.First(x => x.Id == rng.SelectedItemId);
        return new PullRollSelection(selectedRate, isPityForced, rng.RngSeed);
    }

    private sealed record PullRollSelection(
        GachaPoolRewardRate SelectedRate,
        bool IsPityForced,
        string? RngSeed);
}
