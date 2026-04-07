

using System;

namespace TarotNow.Domain.Events;

public class QuestCompletedDomainEvent : IDomainEvent
{
    public Guid UserId { get; }
    public string QuestCode { get; }
    public string PeriodKey { get; }
    public string RewardType { get; }
    public int RewardAmount { get; }
    public DateTime OccurredAtUtc { get; }

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
