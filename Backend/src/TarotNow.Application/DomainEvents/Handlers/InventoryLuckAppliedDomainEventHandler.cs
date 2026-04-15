using System.Globalization;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler hậu xử lý khi item luck được áp dụng.
/// </summary>
public sealed class LuckAppliedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<LuckAppliedDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Khởi tạo handler LuckAppliedDomainEvent.
    /// </summary>
    public LuckAppliedDomainEventHandler(
        INotificationRepository notificationRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _notificationRepository = notificationRepository;
    }

    /// <inheritdoc />
    protected override Task HandleDomainEventAsync(
        LuckAppliedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        return _notificationRepository.CreateAsync(
            new NotificationCreateDto
            {
                UserId = domainEvent.UserId,
                Type = InventoryNotificationTypes.LuckApplied,
                TitleVi = InventoryNotificationTemplates.LuckAppliedTitleVi,
                TitleEn = InventoryNotificationTemplates.LuckAppliedTitleEn,
                TitleZh = InventoryNotificationTemplates.LuckAppliedTitleZh,
                BodyVi = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.LuckAppliedBodyVi,
                    domainEvent.SourceItemCode,
                    domainEvent.LuckValue),
                BodyEn = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.LuckAppliedBodyEn,
                    domainEvent.SourceItemCode,
                    domainEvent.LuckValue),
                BodyZh = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.LuckAppliedBodyZh,
                    domainEvent.SourceItemCode,
                    domainEvent.LuckValue),
            },
            cancellationToken);
    }
}
