/*
 * FILE: AchievementUnlockedDomainEvent.cs
 * MỤC ĐÍCH: Domain event phát ra khi user vừa mở khóa thành tựu lần đầu.
 *   - Kích hoạt notification chúc mừng và/hoặc cấp danh hiệu (Title).
 */

using System;

namespace TarotNow.Domain.Events;

/// <summary>
/// Event sinh ra khi User thỏa điều kiện và được mở khóa Achievement.
/// </summary>
public class AchievementUnlockedDomainEvent : IDomainEvent
{
    public Guid UserId { get; }
    public string AchievementCode { get; }
    public DateTime OccurredAtUtc { get; }

    public AchievementUnlockedDomainEvent(Guid userId, string achievementCode)
    {
        UserId = userId;
        AchievementCode = achievementCode;
        OccurredAtUtc = DateTime.UtcNow;
    }
}
