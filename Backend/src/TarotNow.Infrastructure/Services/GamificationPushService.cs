using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public class GamificationPushService : IGamificationPushService
{
    
    
    
    
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

    public async Task PushCardLevelUpAsync(CardLevelUpPushPayload payload, CancellationToken ct)
    {
        await _pushService.SendEventAsync(
            payload.UserId.ToString(),
            "gamification.card_level_up",
            new { payload.CardId, payload.NewLevel, payload.AtkBonus, payload.DefBonus },
            ct);
    }
}
