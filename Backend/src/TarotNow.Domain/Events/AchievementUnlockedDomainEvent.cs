
using System;

namespace TarotNow.Domain.Events;

// Domain event phát sinh khi người dùng mở khóa thành tựu.
public class AchievementUnlockedDomainEvent : IDomainEvent
{
    // Người dùng mở khóa thành tựu.
    public Guid UserId { get; }

    // Mã thành tựu được mở khóa.
    public string AchievementCode { get; }

    // Thời điểm sự kiện phát sinh (UTC).
    public DateTime OccurredAtUtc { get; }

    /// <summary>
    /// Khởi tạo sự kiện mở khóa thành tựu để publish tới các handler liên quan.
    /// Luồng xử lý: nhận userId/achievementCode và chốt OccurredAtUtc tại thời điểm tạo event.
    /// </summary>
    public AchievementUnlockedDomainEvent(Guid userId, string achievementCode)
    {
        UserId = userId;
        AchievementCode = achievementCode;
        OccurredAtUtc = DateTime.UtcNow;
    }
}
