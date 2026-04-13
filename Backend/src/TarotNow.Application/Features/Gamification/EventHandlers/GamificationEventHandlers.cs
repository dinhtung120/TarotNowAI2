using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Gamification.EventHandlers;

// Handler push thông báo khi quest hoàn thành.
public class QuestCompletedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<QuestCompletedDomainEvent>
{
    private readonly IGamificationPushService _pushService;

    /// <summary>
    /// Khởi tạo handler quest completed domain event.
    /// </summary>
    public QuestCompletedDomainEventHandler(
        IGamificationPushService pushService,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _pushService = pushService;
    }

    /// <summary>
    /// Xử lý domain event quest completed.
    /// </summary>
    protected override async Task HandleDomainEventAsync(
        QuestCompletedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var summary = $"{domainEvent.RewardAmount} {domainEvent.RewardType}";
        await _pushService.PushQuestCompletedAsync(domainEvent.UserId, domainEvent.QuestCode, summary, cancellationToken);
    }
}

// Handler xử lý khi achievement được mở khóa.
public class AchievementUnlockedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AchievementUnlockedDomainEvent>
{
    private readonly IGamificationPushService _pushService;
    private readonly IAchievementRepository _achievementRepository;
    private readonly ITitleRepository _titleRepository;

    /// <summary>
    /// Khởi tạo handler achievement unlocked domain event.
    /// </summary>
    public AchievementUnlockedDomainEventHandler(
        IGamificationPushService pushService,
        IAchievementRepository achievementRepository,
        ITitleRepository titleRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _pushService = pushService;
        _achievementRepository = achievementRepository;
        _titleRepository = titleRepository;
    }

    /// <summary>
    /// Xử lý domain event achievement unlocked.
    /// </summary>
    protected override async Task HandleDomainEventAsync(
        AchievementUnlockedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var definition = await _achievementRepository.GetByCodeAsync(domainEvent.AchievementCode, cancellationToken);
        if (definition == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(definition.GrantsTitleCode) == false)
        {
            await _titleRepository.GrantTitleAsync(domainEvent.UserId, definition.GrantsTitleCode, cancellationToken);
        }

        await _pushService.PushAchievementUnlockedAsync(
            domainEvent.UserId,
            domainEvent.AchievementCode,
            definition.GrantsTitleCode,
            cancellationToken);
    }
}
