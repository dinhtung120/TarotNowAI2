using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Features.Gamification.Dtos;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.Commands;

public record ClaimQuestRewardCommand(Guid UserId, string QuestCode, string PeriodKey) : IRequest<ClaimQuestRewardResult>;

public class ClaimQuestRewardCommandHandler : IRequestHandler<ClaimQuestRewardCommand, ClaimQuestRewardResult>
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
        var prog = await _questRepo.GetProgressAsync(request.UserId, request.QuestCode, request.PeriodKey, cancellationToken);
        if (prog == null || prog.CurrentProgress < prog.Target)
        {
            return new ClaimQuestRewardResult { Success = false }; // Not completed yet
        }

        if (prog.IsClaimed)
        {
            return new ClaimQuestRewardResult { Success = false, AlreadyClaimed = true };
        }

        var quest = await _questRepo.GetQuestByCodeAsync(request.QuestCode, cancellationToken);
        if (quest == null) return new ClaimQuestRewardResult { Success = false };

        // ATOMIC CHECK-AND-SET: Mark claimed early to prevent race condition double-dipping.
        // If TryMarkClaimedAsync returns false, it means someone else (or another request) already claimed it.
        bool markSuccess = await _questRepo.TryMarkClaimedAsync(request.UserId, request.QuestCode, request.PeriodKey, cancellationToken);
        if (!markSuccess)
        {
            return new ClaimQuestRewardResult { Success = false, AlreadyClaimed = true };
        }

        // Process rewards
        string rewardType = string.Empty;
        int rewardAmount = 0;

        foreach (var rew in quest.Rewards)
        {
            // Standardized comparison (addresses Tech Debt #2)
            bool isCurrency = rew.Type.Equals(QuestRewardType.Gold, StringComparison.OrdinalIgnoreCase) 
                           || rew.Type.Equals(QuestRewardType.Diamond, StringComparison.OrdinalIgnoreCase);

            if (isCurrency)
            {
                string rType = rew.Type.Equals(QuestRewardType.Gold, StringComparison.OrdinalIgnoreCase) ? CurrencyType.Gold : CurrencyType.Diamond;
                await _walletRepo.CreditAsync(
                    userId: request.UserId,
                    currency: rType,
                    type: TransactionType.QuestReward,
                    amount: rew.Amount,
                    description: $"Quest reward for {request.QuestCode}",
                    idempotencyKey: $"quest_reward_{request.UserId}_{request.QuestCode}_{request.PeriodKey}",
                    cancellationToken: cancellationToken);
                rewardType = rew.Type;
                rewardAmount = rew.Amount;
            }
            else if (rew.Type.Equals(QuestRewardType.Title, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(rew.TitleCode))
                {
                    await _titleRepo.GrantTitleAsync(request.UserId, rew.TitleCode, cancellationToken);
                }
                rewardType = "Title";
            }
        }

        // Publish Domain Event -> will trigger PushNotification in background
        await _domainEventPublisher.PublishAsync(new QuestCompletedDomainEvent(request.UserId, request.QuestCode, request.PeriodKey, rewardType, rewardAmount), cancellationToken);

        return new ClaimQuestRewardResult { Success = true, RewardType = rewardType, RewardAmount = rewardAmount };
    }
}

public record SetActiveTitleCommand(Guid UserId, string TitleCode) : IRequest<bool>;

public class SetActiveTitleCommandHandler : IRequestHandler<SetActiveTitleCommand, bool>
{
    private readonly ITitleRepository _titleRepo;
    private readonly IUserRepository _userRepo;

    public SetActiveTitleCommandHandler(ITitleRepository titleRepo, IUserRepository userRepo)
    {
        _titleRepo = titleRepo;
        _userRepo = userRepo;
    }

    public async Task<bool> Handle(SetActiveTitleCommand request, CancellationToken cancellationToken)
    {
        // 1. Check ownership
        bool owns = await _titleRepo.OwnsTitleAsync(request.UserId, request.TitleCode, cancellationToken);
        if (!owns && !string.IsNullOrEmpty(request.TitleCode)) return false;

        // 2. Set active
        var user = await _userRepo.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return false;

        user.SetActiveTitle(string.IsNullOrEmpty(request.TitleCode) ? null : request.TitleCode);
        await _userRepo.UpdateAsync(user, cancellationToken);
        
        return true;
    }
}
