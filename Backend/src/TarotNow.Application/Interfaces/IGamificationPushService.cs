using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IGamificationPushService
{
        Task PushQuestCompletedAsync(Guid userId, string questCode, string rewardSummary, CancellationToken ct);
    
        Task PushAchievementUnlockedAsync(Guid userId, string achievementCode, string? grantedTitle, CancellationToken ct);
    
        Task PushCardLevelUpAsync(CardLevelUpPushPayload payload, CancellationToken ct);
}

public sealed record CardLevelUpPushPayload(
    Guid UserId,
    int CardId,
    int NewLevel,
    int AtkBonus,
    int DefBonus);
