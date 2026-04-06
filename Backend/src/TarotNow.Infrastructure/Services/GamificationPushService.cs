using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public class GamificationPushService : IGamificationPushService
{
    // Need SignalR HubContext. Specifically the Gamification or User/Presence hub.
    // Assuming there's an INotificationPushService or we use IHubContext<PresenceHub>.
    // Usually TarotNow uses INotificationPushService for generic stuff, but here we can define the contract.
    // For now, let's keep it as an empty or mockable implementation if HubContext is not fully abstracted here.
    private readonly INotificationPushService _pushService;

    public GamificationPushService(INotificationPushService pushService)
    {
        _pushService = pushService;
    }

    public async Task PushQuestCompletedAsync(Guid userId, string questCode, string rewardSummary, CancellationToken ct)
    {
        await _pushService.SendEventAsync(
            userId.ToString(), 
            "gamification.quest_completed", 
            new { questCode, rewardSummary }, 
            ct);
    }

    public async Task PushAchievementUnlockedAsync(Guid userId, string achievementCode, string? grantedTitle, CancellationToken ct)
    {
        await _pushService.SendEventAsync(
            userId.ToString(),
            "gamification.achievement_unlocked",
            new { achievementCode, grantedTitle },
            ct);
    }

    public async Task PushCardLevelUpAsync(Guid userId, int cardId, int newLevel, int atkBonus, int defBonus, CancellationToken ct)
    {
        await _pushService.SendEventAsync(
            userId.ToString(),
            "gamification.card_level_up",
            new { cardId, newLevel, atkBonus, defBonus },
            ct);
    }
}
