/*
 * FILE: QuestCompletedDomainEvent.cs
 * MỤC ĐÍCH: Domain event phát ra khi user nhận thưởng nhiệm vụ thành công.
 *   - Giúp loose coupling: WalletService, PushNotificationService có thể listen event này.
 */

using System;

namespace TarotNow.Domain.Events;

/// <summary>
/// Event sinh ra sau khi User đã Claim (nhận thưởng) quest thành công.
/// </summary>
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
