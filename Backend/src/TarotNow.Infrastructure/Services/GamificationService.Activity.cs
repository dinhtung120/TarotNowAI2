using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public partial class GamificationService
{
    public async Task OnCheckInAsync(Guid userId, int currentStreak, CancellationToken ct)
    {
        try
        {
            var activeQuests = await GetCachedQuestsAsync(ct);
            await ApplyQuestProgressAsync(
                userId,
                new QuestProgressApplyRequest(activeQuests, "daily_checkin", 1, false),
                ct);
            await ApplyStreakMilestoneProgressAsync(userId, activeQuests, currentStreak, ct);
            await IncrementRankScoresAsync(userId, dailyPoints: 5, monthlyPoints: 5, lifetimePoints: 5, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gamification error OnCheckIn for user {UserId}", userId);
        }
    }

    public async Task OnPostCreatedAsync(Guid userId, CancellationToken ct)
    {
        _logger.LogInformation("[Gamification] Xử lý sự kiện PostCreated cho User: {UserId}", userId);
        try
        {
            var activeQuests = await GetCachedQuestsAsync(ct);
            await ApplyQuestProgressAsync(
                userId,
                new QuestProgressApplyRequest(activeQuests, "PostCreated", 1, false),
                ct);
            await IncrementRankScoresAsync(userId, dailyPoints: 2, monthlyPoints: 0, lifetimePoints: 0, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gamification error OnPostCreated for user {UserId}", userId);
        }
    }

    private async Task ApplyStreakMilestoneProgressAsync(
        Guid userId,
        List<Application.Features.Gamification.Dtos.QuestDefinitionDto> quests,
        int currentStreak,
        CancellationToken ct)
    {
        foreach (var quest in quests.Where(q => q.IsActive && q.TriggerEvent == "streak_reached" && currentStreak >= q.Target))
        {
            var periodKey = Application.Helpers.PeriodKeyHelper.GetPeriodKey(quest.QuestType);
            await _questRepo.UpsertProgressAsync(new QuestProgressUpsertRequest(userId, quest.Code, periodKey, quest.Target, quest.Target), ct);
        }
    }

    private async Task IncrementRankScoresAsync(
        Guid userId,
        long dailyPoints,
        long monthlyPoints,
        long lifetimePoints,
        CancellationToken ct)
    {
        var dailyPeriod = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var monthlyPeriod = DateTime.UtcNow.ToString("yyyy-MM");
        const string lifetimePeriod = "all";

        if (dailyPoints > 0) await _leaderboardRepo.IncrementScoreAsync(userId, "daily_rank_score", dailyPeriod, dailyPoints, ct);
        if (monthlyPoints > 0) await _leaderboardRepo.IncrementScoreAsync(userId, "monthly_rank_score", monthlyPeriod, monthlyPoints, ct);
        if (lifetimePoints > 0) await _leaderboardRepo.IncrementScoreAsync(userId, "lifetime_score", lifetimePeriod, lifetimePoints, ct);
    }
}
