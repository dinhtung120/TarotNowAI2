using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.EventHandlers;

public class QuestCompletedNotification : INotification
{
    public QuestCompletedDomainEvent DomainEvent { get; }
    public QuestCompletedNotification(QuestCompletedDomainEvent domainEvent) => DomainEvent = domainEvent;
}

public class AchievementUnlockedNotification : INotification
{
    public AchievementUnlockedDomainEvent DomainEvent { get; }
    public AchievementUnlockedNotification(AchievementUnlockedDomainEvent domainEvent) => DomainEvent = domainEvent;
}

public class QuestCompletedDomainEventHandler : INotificationHandler<QuestCompletedNotification>
{
    private readonly IGamificationPushService _pushService;

    public QuestCompletedDomainEventHandler(IGamificationPushService pushService)
    {
        _pushService = pushService;
    }

    public async Task Handle(QuestCompletedNotification notification, CancellationToken cancellationToken)
    {
        string summary = $"{notification.DomainEvent.RewardAmount} {notification.DomainEvent.RewardType}";
        await _pushService.PushQuestCompletedAsync(notification.DomainEvent.UserId, notification.DomainEvent.QuestCode, summary, cancellationToken);
    }
}

public class AchievementUnlockedDomainEventHandler : INotificationHandler<AchievementUnlockedNotification>
{
    private readonly IGamificationPushService _pushService;
    private readonly IAchievementRepository _achRepo;
    private readonly ITitleRepository _titleRepo;

    public AchievementUnlockedDomainEventHandler(
        IGamificationPushService pushService,
        IAchievementRepository achRepo,
        ITitleRepository titleRepo)
    {
        _pushService = pushService;
        _achRepo = achRepo;
        _titleRepo = titleRepo;
    }

    public async Task Handle(AchievementUnlockedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        var def = await _achRepo.GetByCodeAsync(ev.AchievementCode, cancellationToken);
        if (def == null) return;

        if (!string.IsNullOrEmpty(def.GrantsTitleCode))
        {
            await _titleRepo.GrantTitleAsync(ev.UserId, def.GrantsTitleCode, cancellationToken);
        }

        await _pushService.PushAchievementUnlockedAsync(ev.UserId, ev.AchievementCode, def.GrantsTitleCode, cancellationToken);
    }
}
