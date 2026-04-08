using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

// Service đẩy sự kiện gamification ra kênh notification realtime.
public class GamificationPushService : IGamificationPushService
{
    // Cổng push event dùng chung cho nhiều loại thông báo hệ thống.
    private readonly INotificationPushService _pushService;

    /// <summary>
    /// Khởi tạo service push gamification.
    /// Luồng inject abstraction giúp thay backend push mà không đổi logic nghiệp vụ.
    /// </summary>
    public GamificationPushService(INotificationPushService pushService)
    {
        _pushService = pushService;
    }

    /// <summary>
    /// Gửi thông báo hoàn thành quest cho người dùng.
    /// Luồng chuẩn hóa tên event để frontend bắt đúng luồng gamification.
    /// </summary>
    public async Task PushQuestCompletedAsync(Guid userId, string questCode, string rewardSummary, CancellationToken ct)
    {
        await _pushService.SendEventAsync(
            userId.ToString(),
            "gamification.quest_completed",
            new { questCode, rewardSummary },
            ct);
    }

    /// <summary>
    /// Gửi thông báo mở khóa achievement.
    /// Luồng này giúp client cập nhật UI thành tựu ngay sau khi backend xác nhận.
    /// </summary>
    public async Task PushAchievementUnlockedAsync(Guid userId, string achievementCode, string? grantedTitle, CancellationToken ct)
    {
        await _pushService.SendEventAsync(
            userId.ToString(),
            "gamification.achievement_unlocked",
            new { achievementCode, grantedTitle },
            ct);
    }

    /// <summary>
    /// Gửi thông báo thăng cấp thẻ bài.
    /// Luồng truyền payload đầy đủ để client hiển thị chỉ số tăng cụ thể.
    /// </summary>
    public async Task PushCardLevelUpAsync(CardLevelUpPushPayload payload, CancellationToken ct)
    {
        await _pushService.SendEventAsync(
            payload.UserId.ToString(),
            "gamification.card_level_up",
            new { payload.CardId, payload.NewLevel, payload.AtkBonus, payload.DefBonus },
            ct);
    }
}
