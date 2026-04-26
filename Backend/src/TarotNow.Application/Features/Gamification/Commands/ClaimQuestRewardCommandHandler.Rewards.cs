using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.Commands;

public partial class ClaimQuestRewardCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Cấp toàn bộ phần thưởng của quest.
    /// Luồng xử lý: duyệt danh sách reward, reward tiền tệ thì credit ví, reward title thì grant title; đồng thời cập nhật reward summary cuối cùng.
    /// </summary>
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
                // Ghi nhận summary theo reward vừa xử lý để trả về client.
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

    /// <summary>
    /// Cộng thưởng tiền tệ (Gold/Diamond) cho quest.
    /// Luồng xử lý: xác định currency từ reward type và gọi wallet credit với idempotency key theo reward index.
    /// </summary>
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
        await _domainEventPublisher.PublishAsync(
            new MoneyChangedDomainEvent
            {
                UserId = request.UserId,
                Currency = currency,
                ChangeType = TransactionType.QuestReward,
                DeltaAmount = reward.Amount,
                ReferenceId = $"{request.QuestCode}:{request.PeriodKey}:{index}"
            },
            cancellationToken);
    }

    /// <summary>
    /// Cấp title reward cho user nếu quest reward có TitleCode.
    /// Luồng xử lý: bỏ qua khi TitleCode rỗng, ngược lại grant title qua title repository.
    /// </summary>
    private async Task GrantQuestTitleAsync(Guid userId, string? titleCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(titleCode))
        {
            await _domainEventPublisher.PublishAsync(
                new TitleGrantedDomainEvent
                {
                    UserId = userId,
                    TitleCode = titleCode,
                    Source = "quest_reward"
                },
                cancellationToken);
        }
    }

    /// <summary>
    /// Phát domain event sau khi claim reward thành công.
    /// Luồng xử lý: publish QuestCompletedDomainEvent với thông tin reward summary để downstream xử lý.
    /// </summary>
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

    /// <summary>
    /// Kiểm tra reward type có phải dạng tiền tệ hay không.
    /// Luồng xử lý: so sánh không phân biệt hoa thường với Gold/Diamond.
    /// </summary>
    private static bool IsCurrencyReward(string rewardType)
        => rewardType.Equals(QuestRewardType.Gold, StringComparison.OrdinalIgnoreCase)
           || rewardType.Equals(QuestRewardType.Diamond, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Dựng idempotency key cho từng reward item khi credit.
    /// Luồng xử lý: chuẩn hóa type/title và ghép thông tin request + reward index để tạo khóa duy nhất, chống credit trùng.
    /// </summary>
    private static string BuildRewardIdempotencyKey(ClaimQuestRewardCommand request, QuestRewardItemDto reward, int index)
    {
        var normalizedType = reward.Type.Trim().ToLowerInvariant();
        var titleCode = reward.TitleCode?.Trim().ToLowerInvariant() ?? "na";
        return $"quest_reward_{request.UserId:N}_{request.QuestCode}_{request.PeriodKey}_{index}_{normalizedType}_{reward.Amount}_{titleCode}";
    }

    // Tóm tắt phần thưởng đã cấp dùng cho response và domain event.
    private readonly record struct RewardSummary(string RewardType, int RewardAmount)
    {
        // Trạng thái mặc định khi chưa cấp phần thưởng nào.
        public static RewardSummary Empty => new(string.Empty, 0);
    }
}
