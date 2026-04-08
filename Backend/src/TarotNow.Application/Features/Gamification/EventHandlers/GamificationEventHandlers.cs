using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.EventHandlers;

// Notification bọc domain event khi quest hoàn thành.
public class QuestCompletedNotification : INotification
{
    // Domain event quest completed gốc.
    public QuestCompletedDomainEvent DomainEvent { get; }

    /// <summary>
    /// Khởi tạo notification quest completed.
    /// Luồng xử lý: gói domain event vào notification để MediatR dispatch.
    /// </summary>
    public QuestCompletedNotification(QuestCompletedDomainEvent domainEvent) => DomainEvent = domainEvent;
}

// Notification bọc domain event khi achievement được mở khóa.
public class AchievementUnlockedNotification : INotification
{
    // Domain event achievement unlocked gốc.
    public AchievementUnlockedDomainEvent DomainEvent { get; }

    /// <summary>
    /// Khởi tạo notification achievement unlocked.
    /// Luồng xử lý: gói domain event vào notification để MediatR dispatch.
    /// </summary>
    public AchievementUnlockedNotification(AchievementUnlockedDomainEvent domainEvent) => DomainEvent = domainEvent;
}

// Handler push thông báo khi quest hoàn thành.
public class QuestCompletedDomainEventHandler : INotificationHandler<QuestCompletedNotification>
{
    private readonly IGamificationPushService _pushService;

    /// <summary>
    /// Khởi tạo handler quest completed notification.
    /// Luồng xử lý: nhận push service để gửi realtime/in-app notification.
    /// </summary>
    public QuestCompletedDomainEventHandler(IGamificationPushService pushService)
    {
        _pushService = pushService;
    }

    /// <summary>
    /// Xử lý notification quest completed.
    /// Luồng xử lý: dựng summary phần thưởng rồi push thông báo hoàn thành quest cho user.
    /// </summary>
    public async Task Handle(QuestCompletedNotification notification, CancellationToken cancellationToken)
    {
        string summary = $"{notification.DomainEvent.RewardAmount} {notification.DomainEvent.RewardType}";
        await _pushService.PushQuestCompletedAsync(notification.DomainEvent.UserId, notification.DomainEvent.QuestCode, summary, cancellationToken);
    }
}

// Handler xử lý khi achievement được mở khóa.
public class AchievementUnlockedDomainEventHandler : INotificationHandler<AchievementUnlockedNotification>
{
    private readonly IGamificationPushService _pushService;
    private readonly IAchievementRepository _achRepo;
    private readonly ITitleRepository _titleRepo;

    /// <summary>
    /// Khởi tạo handler achievement unlocked notification.
    /// Luồng xử lý: nhận push service và repository achievement/title để cấp title thưởng và đẩy thông báo.
    /// </summary>
    public AchievementUnlockedDomainEventHandler(
        IGamificationPushService pushService,
        IAchievementRepository achRepo,
        ITitleRepository titleRepo)
    {
        _pushService = pushService;
        _achRepo = achRepo;
        _titleRepo = titleRepo;
    }

    /// <summary>
    /// Xử lý notification achievement unlocked.
    /// Luồng xử lý: tải định nghĩa achievement, cấp title nếu có GrantsTitleCode, rồi push thông báo mở khóa achievement.
    /// </summary>
    public async Task Handle(AchievementUnlockedNotification notification, CancellationToken cancellationToken)
    {
        var ev = notification.DomainEvent;
        var def = await _achRepo.GetByCodeAsync(ev.AchievementCode, cancellationToken);
        if (def == null)
        {
            // Edge case: không còn định nghĩa achievement tương ứng.
            return;
        }

        if (!string.IsNullOrEmpty(def.GrantsTitleCode))
        {
            // Cấp title thưởng đi kèm achievement nếu cấu hình có.
            await _titleRepo.GrantTitleAsync(ev.UserId, def.GrantsTitleCode, cancellationToken);
        }

        await _pushService.PushAchievementUnlockedAsync(ev.UserId, ev.AchievementCode, def.GrantsTitleCode, cancellationToken);
    }
}
