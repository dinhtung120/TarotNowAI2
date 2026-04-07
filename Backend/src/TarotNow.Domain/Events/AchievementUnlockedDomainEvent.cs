

using System;

namespace TarotNow.Domain.Events;

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
