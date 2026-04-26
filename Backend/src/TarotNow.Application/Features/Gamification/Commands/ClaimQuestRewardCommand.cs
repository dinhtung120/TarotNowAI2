using MediatR;
using System;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Gamification.Commands;

// Command nhận thưởng quest theo user/quest/period.
public record ClaimQuestRewardCommand(Guid UserId, string QuestCode, string PeriodKey) : IRequest<ClaimQuestRewardResult>;

// Handler xử lý luồng claim reward của quest.
public partial class ClaimQuestRewardCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ClaimQuestRewardCommandHandlerRequestedDomainEvent>
{
    private readonly IQuestRepository _questRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler claim quest reward.
    /// Luồng xử lý: nhận repository quest/wallet/title và domain event publisher để xử lý claim + trả thưởng + phát sự kiện.
    /// </summary>
    public ClaimQuestRewardCommandHandlerRequestedDomainEventHandler(
        IQuestRepository questRepo,
        IWalletRepository walletRepo,
        IDomainEventPublisher domainEventPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _questRepo = questRepo;
        _walletRepo = walletRepo;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command nhận thưởng quest.
    /// Luồng xử lý: kiểm tra điều kiện claim, khóa trạng thái claimed, cấp thưởng, phát sự kiện hoàn thành quest; nếu lỗi thì rollback cờ claimed.
    /// </summary>
    public async Task<ClaimQuestRewardResult> Handle(ClaimQuestRewardCommand request, CancellationToken cancellationToken)
    {
        var progress = await _questRepo.GetProgressAsync(request.UserId, request.QuestCode, request.PeriodKey, cancellationToken);
        if (!CanClaim(progress))
        {
            // Không đủ tiến độ claim thì trả thất bại business.
            return new ClaimQuestRewardResult { Success = false };
        }

        if (progress!.IsClaimed)
        {
            // Quest đã claimed trước đó thì trả trạng thái idempotent.
            return new ClaimQuestRewardResult { Success = false, AlreadyClaimed = true };
        }

        var quest = await _questRepo.GetQuestByCodeAsync(request.QuestCode, cancellationToken);
        if (quest == null || quest.Rewards.Count == 0)
        {
            // Cấu hình quest/reward không hợp lệ nên không thể cấp thưởng.
            return new ClaimQuestRewardResult { Success = false };
        }

        var claimLocked = await _questRepo.TryMarkClaimedAsync(request.UserId, request.QuestCode, request.PeriodKey, cancellationToken);
        if (!claimLocked)
        {
            // Concurrent claim: request khác đã claim trước nên trả already claimed.
            return new ClaimQuestRewardResult { Success = false, AlreadyClaimed = true };
        }

        try
        {
            var rewardSummary = await GrantRewardsAsync(request, quest.Rewards, cancellationToken);
            await PublishQuestCompletedAsync(request, rewardSummary, cancellationToken);
            return new ClaimQuestRewardResult { Success = true, RewardType = rewardSummary.RewardType, RewardAmount = rewardSummary.RewardAmount };
        }
        catch
        {
            // Nếu cấp thưởng lỗi, rollback cờ claimed để user có thể claim lại.
            await _questRepo.RevertClaimedAsync(request.UserId, request.QuestCode, request.PeriodKey, cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Kiểm tra quest progress có đủ điều kiện claim hay không.
    /// Luồng xử lý: yêu cầu progress tồn tại và current progress đạt ngưỡng target.
    /// </summary>
    private static bool CanClaim(QuestProgressDto? progress)
        => progress != null && progress.CurrentProgress >= progress.Target;

    protected override async Task HandleDomainEventAsync(
        ClaimQuestRewardCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
