using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IGamificationPushService
{
    /// <summary>Push event "gamification.quest_completed" qua SignalR</summary>
    Task PushQuestCompletedAsync(Guid userId, string questCode, string rewardSummary, CancellationToken ct);
    
    /// <summary>Push event "gamification.achievement_unlocked" qua SignalR</summary>
    Task PushAchievementUnlockedAsync(Guid userId, string achievementCode, string? grantedTitle, CancellationToken ct);
    
    /// <summary>Push event "gamification.card_level_up" qua SignalR — khi card level up</summary>
    Task PushCardLevelUpAsync(CardLevelUpPushPayload payload, CancellationToken ct);
}

public sealed record CardLevelUpPushPayload(
    Guid UserId,
    int CardId,
    int NewLevel,
    int AtkBonus,
    int DefBonus);
