using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Helpers;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public partial class GamificationService
{
    public async Task OnReadingCompletedAsync(Guid userId, CancellationToken ct)
    {
        try
        {
            var activeQuests = await GetCachedQuestsAsync(ct);
            await ApplyQuestProgressAsync(
                userId,
                new QuestProgressApplyRequest(activeQuests, "ReadingCompleted", 1, false),
                ct);
            await IncrementRankScoresAsync(userId, dailyPoints: 10, monthlyPoints: 10, lifetimePoints: 10, ct);
            await CheckAndUnlockAchievementsAsync(userId, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gamification error OnReadingCompleted for user {UserId}", userId);
        }
    }

    private async Task CheckAndUnlockAchievementsAsync(Guid userId, CancellationToken ct)
    {
        var allDefs = await _achievementRepo.GetAllAchievementsAsync(ct);
        var unlockedCodes = (await _achievementRepo.GetUserAchievementsAsync(userId, ct))
            .Select(u => u.AchievementCode)
            .ToHashSet();

        foreach (var definition in allDefs)
        {
            if (unlockedCodes.Contains(definition.Code)) continue;
            if (!ShouldUnlockAchievement(definition.Code)) continue;
            await _achievementRepo.UnlockAsync(userId, definition.Code, ct);
            await _pushService.PushAchievementUnlockedAsync(userId, definition.Code, definition.GrantsTitleCode, ct);
        }
    }

    private static bool ShouldUnlockAchievement(string achievementCode)
        => achievementCode == "first_reading";

    private async Task<List<Application.Features.Gamification.Dtos.QuestDefinitionDto>> GetCachedQuestsAsync(CancellationToken ct)
    {
        return await _cache.GetOrCreateAsync("gamification_quests_all", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return _questRepo.GetAllQuestsAsync(ct);
        }) ?? new List<Application.Features.Gamification.Dtos.QuestDefinitionDto>();
    }

    private async Task ApplyQuestProgressAsync(
        Guid userId,
        QuestProgressApplyRequest request,
        CancellationToken ct)
    {
        foreach (var quest in request.Quests.Where(q => q.IsActive && q.TriggerEvent == request.TriggerEvent))
        {
            var periodKey = PeriodKeyHelper.GetPeriodKey(quest.QuestType);
            var increment = request.UseTargetAsIncrement ? quest.Target : request.Progress;
            await _questRepo.UpsertProgressAsync(new QuestProgressUpsertRequest(userId, quest.Code, periodKey, quest.Target, increment), ct);
        }
    }

    private sealed record QuestProgressApplyRequest(
        List<Application.Features.Gamification.Dtos.QuestDefinitionDto> Quests,
        string TriggerEvent,
        int Progress,
        bool UseTargetAsIncrement);
}
