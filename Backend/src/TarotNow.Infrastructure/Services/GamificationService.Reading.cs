using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Helpers;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public partial class GamificationService
{
    /// <summary>
    /// Xử lý gamification khi người dùng hoàn thành một phiên đọc bài.
    /// Luồng cập nhật quest, cộng điểm leaderboard và kiểm tra mở khóa achievement.
    /// </summary>
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
            // Không để lỗi side-effect gamification làm fail nghiệp vụ đọc bài chính.
            _logger.LogError(ex, "Gamification error OnReadingCompleted for user {UserId}", userId);
        }
    }

    /// <summary>
    /// Kiểm tra và mở khóa các achievement chưa đạt của người dùng.
    /// Luồng tải toàn bộ definition, loại bỏ mã đã mở khóa, rồi đánh giá điều kiện mở khóa từng mục.
    /// </summary>
    private async Task CheckAndUnlockAchievementsAsync(Guid userId, CancellationToken ct)
    {
        var allDefs = await _achievementRepo.GetAllAchievementsAsync(ct);
        var unlockedCodes = (await _achievementRepo.GetUserAchievementsAsync(userId, ct))
            .Select(u => u.AchievementCode)
            .ToHashSet();

        foreach (var definition in allDefs)
        {
            // Bỏ qua achievement đã có để tránh ghi trùng và push trùng thông báo.
            if (unlockedCodes.Contains(definition.Code)) continue;
            // Chỉ unlock khi rule nghiệp vụ cho phép để bảo toàn logic tiến trình.
            if (!ShouldUnlockAchievement(definition.Code)) continue;
            await _achievementRepo.UnlockAsync(userId, definition.Code, ct);
            // Push ngay sau unlock để client hiển thị phần thưởng tức thời.
            await _pushService.PushAchievementUnlockedAsync(userId, definition.Code, definition.GrantsTitleCode, ct);
        }
    }

    /// <summary>
    /// Đánh giá điều kiện mở khóa achievement theo mã định danh.
    /// Luồng hiện tại chỉ mở khóa mốc đọc bài đầu tiên theo policy đơn giản.
    /// </summary>
    private static bool ShouldUnlockAchievement(string achievementCode)
        => achievementCode == "first_reading";

    /// <summary>
    /// Lấy danh sách quest active từ cache hoặc repository.
    /// Luồng cache 5 phút giúp giảm truy vấn lặp cho các sự kiện gamification tần suất cao.
    /// </summary>
    private async Task<List<Application.Features.Gamification.Dtos.QuestDefinitionDto>> GetCachedQuestsAsync(CancellationToken ct)
    {
        return await _cache.GetOrCreateAsync("gamification_quests_all", entry =>
        {
            // TTL ngắn để vừa tiết kiệm truy vấn vừa phản ánh cập nhật quest tương đối nhanh.
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return _questRepo.GetAllQuestsAsync(ct);
        }) ?? new List<Application.Features.Gamification.Dtos.QuestDefinitionDto>();
    }

    /// <summary>
    /// Áp dụng tiến độ cho các quest phù hợp trigger sự kiện hiện tại.
    /// Luồng lọc quest active rồi upsert tiến độ theo period key tương ứng từng loại quest.
    /// </summary>
    private async Task ApplyQuestProgressAsync(
        Guid userId,
        QuestProgressApplyRequest request,
        CancellationToken ct)
    {
        foreach (var quest in request.Quests.Where(q => q.IsActive && q.TriggerEvent == request.TriggerEvent))
        {
            var periodKey = PeriodKeyHelper.GetPeriodKey(quest.QuestType);
            // Một số quest cần cộng bằng target để hoàn thành ngay theo design nghiệp vụ.
            var increment = request.UseTargetAsIncrement ? quest.Target : request.Progress;
            await _questRepo.UpsertProgressAsync(new QuestProgressUpsertRequest(userId, quest.Code, periodKey, quest.Target, increment), ct);
        }
    }

    // Payload nội bộ gom tham số áp dụng tiến độ quest để tránh truyền nhiều biến rời rạc.
    private sealed record QuestProgressApplyRequest(
        List<Application.Features.Gamification.Dtos.QuestDefinitionDto> Quests,
        string TriggerEvent,
        int Progress,
        bool UseTargetAsIncrement);
}
