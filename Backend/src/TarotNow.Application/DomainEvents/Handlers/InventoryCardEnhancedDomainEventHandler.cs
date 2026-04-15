using System.Globalization;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler hậu xử lý khi card được enhancement từ inventory item.
/// </summary>
public sealed class CardEnhancedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<CardEnhancedDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Khởi tạo handler card enhanced.
    /// </summary>
    public CardEnhancedDomainEventHandler(
        INotificationRepository notificationRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _notificationRepository = notificationRepository;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        CardEnhancedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var levelUpFlag = domainEvent.UpgradeSucceeded ? 1 : 0;
        return _notificationRepository.CreateAsync(
            new NotificationCreateDto
            {
                UserId = domainEvent.UserId,
                Type = InventoryNotificationTypes.CardEnhanced,
                TitleVi = InventoryNotificationTemplates.CardEnhancedTitleVi,
                TitleEn = InventoryNotificationTemplates.CardEnhancedTitleEn,
                TitleZh = InventoryNotificationTemplates.CardEnhancedTitleZh,
                BodyVi = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.CardEnhancedBodyVi,
                    domainEvent.CardId,
                    domainEvent.ExpDelta,
                    domainEvent.AttackDelta,
                    domainEvent.DefenseDelta,
                    levelUpFlag),
                BodyEn = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.CardEnhancedBodyEn,
                    domainEvent.CardId,
                    domainEvent.ExpDelta,
                    domainEvent.AttackDelta,
                    domainEvent.DefenseDelta,
                    levelUpFlag),
                BodyZh = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.CardEnhancedBodyZh,
                    domainEvent.CardId,
                    domainEvent.ExpDelta,
                    domainEvent.AttackDelta,
                    domainEvent.DefenseDelta,
                    levelUpFlag),
            },
            cancellationToken);
    }
}
