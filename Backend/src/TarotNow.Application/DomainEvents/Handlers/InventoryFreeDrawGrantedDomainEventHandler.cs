using System.Globalization;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler hậu xử lý khi hệ thống cấp free draw từ inventory.
/// </summary>
public sealed class FreeDrawGrantedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<FreeDrawGrantedDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Khởi tạo handler FreeDrawGrantedDomainEvent.
    /// </summary>
    public FreeDrawGrantedDomainEventHandler(
        INotificationRepository notificationRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _notificationRepository = notificationRepository;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        FreeDrawGrantedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _notificationRepository.CreateAsync(
            new NotificationCreateDto
            {
                UserId = domainEvent.UserId,
                Type = InventoryNotificationTypes.FreeDrawGranted,
                TitleVi = InventoryNotificationTemplates.FreeDrawTitleVi,
                TitleEn = InventoryNotificationTemplates.FreeDrawTitleEn,
                TitleZh = InventoryNotificationTemplates.FreeDrawTitleZh,
                BodyVi = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.FreeDrawBodyVi,
                    domainEvent.GrantedCount,
                    domainEvent.SourceItemCode),
                BodyEn = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.FreeDrawBodyEn,
                    domainEvent.GrantedCount,
                    domainEvent.SourceItemCode),
                BodyZh = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.FreeDrawBodyZh,
                    domainEvent.GrantedCount,
                    domainEvent.SourceItemCode),
            },
            cancellationToken);
    }
}
