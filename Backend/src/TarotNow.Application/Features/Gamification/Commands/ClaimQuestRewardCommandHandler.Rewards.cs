using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.Commands;

public partial class ClaimQuestRewardCommandHandler
{
    private async Task<RewardSummary> GrantRewardsAsync(
        ClaimQuestRewardCommand request,
        List<QuestRewardItemDto> rewards,
        CancellationToken cancellationToken)
    {
        var summary = RewardSummary.Empty;
        for (var index = 0; index < rewards.Count; index++)
        {
            var reward = rewards[index];
            if (IsCurrencyReward(reward.Type))
            {
                await CreditQuestCurrencyAsync(request, reward, index, cancellationToken);
                summary = new RewardSummary(reward.Type, reward.Amount);
                continue;
            }

            if (reward.Type.Equals(QuestRewardType.Title, StringComparison.OrdinalIgnoreCase))
            {
                await GrantQuestTitleAsync(request.UserId, reward.TitleCode, cancellationToken);
                summary = new RewardSummary("Title", reward.Amount);
            }
        }

        return summary;
    }

    private async Task CreditQuestCurrencyAsync(
        ClaimQuestRewardCommand request,
        QuestRewardItemDto reward,
        int index,
        CancellationToken cancellationToken)
    {
        var currency = reward.Type.Equals(QuestRewardType.Gold, StringComparison.OrdinalIgnoreCase)
            ? CurrencyType.Gold
            : CurrencyType.Diamond;

        await _walletRepo.CreditAsync(
            userId: request.UserId,
            currency: currency,
            type: TransactionType.QuestReward,
            amount: reward.Amount,
            description: $"Quest reward for {request.QuestCode}",
            idempotencyKey: BuildRewardIdempotencyKey(request, reward, index),
            cancellationToken: cancellationToken);
    }

    private async Task GrantQuestTitleAsync(Guid userId, string? titleCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(titleCode))
        {
            await _titleRepo.GrantTitleAsync(userId, titleCode, cancellationToken);
        }
    }

    private async Task PublishQuestCompletedAsync(
        ClaimQuestRewardCommand request,
        RewardSummary rewardSummary,
        CancellationToken cancellationToken)
    {
        await _domainEventPublisher.PublishAsync(
            new QuestCompletedDomainEvent(
                request.UserId,
                request.QuestCode,
                request.PeriodKey,
                rewardSummary.RewardType,
                rewardSummary.RewardAmount),
            cancellationToken);
    }

    private static bool IsCurrencyReward(string rewardType)
        => rewardType.Equals(QuestRewardType.Gold, StringComparison.OrdinalIgnoreCase)
           || rewardType.Equals(QuestRewardType.Diamond, StringComparison.OrdinalIgnoreCase);

    private static string BuildRewardIdempotencyKey(ClaimQuestRewardCommand request, QuestRewardItemDto reward, int index)
    {
        var normalizedType = reward.Type.Trim().ToLowerInvariant();
        var titleCode = reward.TitleCode?.Trim().ToLowerInvariant() ?? "na";
        return $"quest_reward_{request.UserId:N}_{request.QuestCode}_{request.PeriodKey}_{index}_{normalizedType}_{reward.Amount}_{titleCode}";
    }

    private readonly record struct RewardSummary(string RewardType, int RewardAmount)
    {
        public static RewardSummary Empty => new(string.Empty, 0);
    }
}
