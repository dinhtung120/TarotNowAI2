using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract đẩy thông báo gamification theo sự kiện để tăng phản hồi thời gian thực cho người dùng.
public interface IGamificationPushService
{
    /// <summary>
    /// Đẩy thông báo hoàn thành nhiệm vụ để người dùng nhận phản hồi ngay sau khi đạt điều kiện.
    /// Luồng xử lý: gửi event chứa questCode và rewardSummary tới kênh realtime của userId.
    /// </summary>
    Task PushQuestCompletedAsync(Guid userId, string questCode, string rewardSummary, CancellationToken ct);

    /// <summary>
    /// Đẩy thông báo mở khóa thành tựu để cập nhật UI và phần thưởng danh hiệu.
    /// Luồng xử lý: publish sự kiện achievement theo userId cùng title được cấp (nếu có).
    /// </summary>
    Task PushAchievementUnlockedAsync(Guid userId, string achievementCode, string? grantedTitle, CancellationToken ct);

    /// <summary>
    /// Đẩy thông báo nâng cấp thẻ bài để đồng bộ chỉ số mới trên client.
    /// Luồng xử lý: phát payload tăng cấp đã chuẩn hóa tới kênh người dùng.
    /// </summary>
    Task PushCardLevelUpAsync(CardLevelUpPushPayload payload, CancellationToken ct);
}

// Payload thông báo nâng cấp thẻ bài cho kênh realtime.
public sealed record CardLevelUpPushPayload(
    Guid UserId,
    int CardId,
    int NewLevel,
    int AtkBonus,
    int DefBonus);
