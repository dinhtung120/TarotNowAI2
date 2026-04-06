using MediatR;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Gamification.Commands;

public record ClaimQuestRewardCommand(Guid UserId, string QuestCode, string PeriodKey) : IRequest<ClaimQuestRewardResult>;

public partial class ClaimQuestRewardCommandHandler : IRequestHandler<ClaimQuestRewardCommand, ClaimQuestRewardResult>
{
    private readonly IQuestRepository _questRepo;
    private readonly IWalletRepository _walletRepo;
    private readonly ITitleRepository _titleRepo;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public ClaimQuestRewardCommandHandler(
        IQuestRepository questRepo,
        IWalletRepository walletRepo,
        ITitleRepository titleRepo,
        IDomainEventPublisher domainEventPublisher)
    {
        _questRepo = questRepo;
        _walletRepo = walletRepo;
        _titleRepo = titleRepo;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<ClaimQuestRewardResult> Handle(ClaimQuestRewardCommand request, CancellationToken cancellationToken)
    {
        var progress = await _questRepo.GetProgressAsync(request.UserId, request.QuestCode, request.PeriodKey, cancellationToken);
        if (!CanClaim(progress)) return new ClaimQuestRewardResult { Success = false };
        if (progress!.IsClaimed) return new ClaimQuestRewardResult { Success = false, AlreadyClaimed = true };

        var quest = await _questRepo.GetQuestByCodeAsync(request.QuestCode, cancellationToken);
        if (quest == null || quest.Rewards.Count == 0) return new ClaimQuestRewardResult { Success = false };

        var claimLocked = await _questRepo.TryMarkClaimedAsync(request.UserId, request.QuestCode, request.PeriodKey, cancellationToken);
        if (!claimLocked) return new ClaimQuestRewardResult { Success = false, AlreadyClaimed = true };

        try
        {
            var rewardSummary = await GrantRewardsAsync(request, quest.Rewards, cancellationToken);
            await PublishQuestCompletedAsync(request, rewardSummary, cancellationToken);
            return new ClaimQuestRewardResult { Success = true, RewardType = rewardSummary.RewardType, RewardAmount = rewardSummary.RewardAmount };
        }
        catch
        {
            await _questRepo.RevertClaimedAsync(request.UserId, request.QuestCode, request.PeriodKey, cancellationToken);
            throw;
        }
    }

    private static bool CanClaim(QuestProgressDto? progress)
        => progress != null && progress.CurrentProgress >= progress.Target;
}
