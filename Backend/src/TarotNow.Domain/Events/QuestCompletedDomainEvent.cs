
using System;

namespace TarotNow.Domain.Events;

// Domain event phát sinh khi người dùng hoàn tất một quest và nhận thưởng.
public class QuestCompletedDomainEvent : IDomainEvent
{
    // Người dùng hoàn tất quest.
    public Guid UserId { get; }

    // Mã quest hoàn tất.
    public string QuestCode { get; }

    // Kỳ quest áp dụng (daily/weekly/...).
    public string PeriodKey { get; }

    // Loại phần thưởng.
    public string RewardType { get; }

    // Giá trị phần thưởng.
    public int RewardAmount { get; }

    // Thời điểm phát sinh sự kiện (UTC).
    public DateTime OccurredAtUtc { get; }

    /// <summary>
    /// Khởi tạo sự kiện quest completed để các handler hậu xử lý nhận thưởng chạy đồng bộ.
    /// Luồng xử lý: nhận dữ liệu quest/reward và chốt OccurredAtUtc tại thời điểm tạo event.
    /// </summary>
    public QuestCompletedDomainEvent(
        Guid userId,
        string questCode,
        string periodKey,
        string rewardType,
        int rewardAmount)
    {
        UserId = userId;
        QuestCode = questCode;
        PeriodKey = periodKey;
        RewardType = rewardType;
        RewardAmount = rewardAmount;
        OccurredAtUtc = DateTime.UtcNow;
    }
}
