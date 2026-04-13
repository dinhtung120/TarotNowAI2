using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public partial class GamificationService
{
    /// <summary>
    /// Xử lý gamification khi người dùng check-in hằng ngày.
    /// Luồng cập nhật tiến độ quest, mốc streak và cộng điểm bảng xếp hạng.
    /// </summary>
    public async Task OnCheckInAsync(Guid userId, int currentStreak, CancellationToken ct)
    {
        var activeQuests = await GetCachedQuestsAsync(ct);
        await ApplyQuestProgressAsync(
            userId,
            new QuestProgressApplyRequest(activeQuests, "daily_checkin", 1, false),
            ct);
        await ApplyStreakMilestoneProgressAsync(userId, activeQuests, currentStreak, ct);
        await IncrementRankScoresAsync(userId, dailyPoints: 5, monthlyPoints: 5, lifetimePoints: 5, ct);
    }

    /// <summary>
    /// Xử lý gamification khi người dùng tạo bài viết cộng đồng.
    /// Luồng cộng tiến độ quest theo sự kiện PostCreated và tăng điểm daily leaderboard.
    /// </summary>
    public async Task OnPostCreatedAsync(Guid userId, CancellationToken ct)
    {
        // Log thông tin vào mức Information để hỗ trợ truy vết hoạt động cộng đồng.
        _logger.LogInformation("[Gamification] Xử lý sự kiện PostCreated cho User: {UserId}", userId);
        var activeQuests = await GetCachedQuestsAsync(ct);
        await ApplyQuestProgressAsync(
            userId,
            new QuestProgressApplyRequest(activeQuests, "PostCreated", 1, false),
            ct);
        await IncrementRankScoresAsync(userId, dailyPoints: 2, monthlyPoints: 0, lifetimePoints: 0, ct);
    }

    /// <summary>
    /// Cập nhật tiến độ các quest mốc streak đạt điều kiện.
    /// Luồng lọc quest active đúng trigger và chỉ upsert khi streak hiện tại đạt target.
    /// </summary>
    private async Task ApplyStreakMilestoneProgressAsync(
        Guid userId,
        List<Application.Features.Gamification.Dtos.QuestDefinitionDto> quests,
        int currentStreak,
        CancellationToken ct)
    {
        foreach (var quest in quests.Where(q => q.IsActive && q.TriggerEvent == "streak_reached" && currentStreak >= q.Target))
        {
            // Dùng period key theo quest type để dữ liệu tiến độ được tổng hợp đúng kỳ.
            var periodKey = Application.Helpers.PeriodKeyHelper.GetPeriodKey(quest.QuestType);
            await _questRepo.UpsertProgressAsync(new QuestProgressUpsertRequest(userId, quest.Code, periodKey, quest.Target, quest.Target), ct);
        }
    }

    /// <summary>
    /// Cộng điểm leaderboard cho các bảng xếp hạng ngày/tháng/tổng.
    /// Luồng chỉ ghi điểm khi giá trị > 0 để tránh tạo bản ghi thừa.
    /// </summary>
    private async Task IncrementRankScoresAsync(
        Guid userId,
        long dailyPoints,
        long monthlyPoints,
        long lifetimePoints,
        CancellationToken ct)
    {
        var dailyPeriod = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var monthlyPeriod = DateTime.UtcNow.ToString("yyyy-MM");
        // Key cố định cho bảng lifetime để cộng dồn toàn thời gian.
        const string lifetimePeriod = "all";

        if (dailyPoints > 0) await _leaderboardRepo.IncrementScoreAsync(userId, "daily_rank_score", dailyPeriod, dailyPoints, ct);
        if (monthlyPoints > 0) await _leaderboardRepo.IncrementScoreAsync(userId, "monthly_rank_score", monthlyPeriod, monthlyPoints, ct);
        if (lifetimePoints > 0) await _leaderboardRepo.IncrementScoreAsync(userId, "lifetime_score", lifetimePeriod, lifetimePoints, ct);
    }
}
