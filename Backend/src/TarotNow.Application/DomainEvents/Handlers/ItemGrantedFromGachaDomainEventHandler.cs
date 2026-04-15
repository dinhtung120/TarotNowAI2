using System.Globalization;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Gacha;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler side-effects phụ khi gacha cấp item cho người dùng.
/// </summary>
public sealed class ItemGrantedFromGachaDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ItemGrantedFromGachaDomainEvent>
{
    private readonly IItemDefinitionRepository _itemDefinitionRepository;
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Khởi tạo handler.
    /// </summary>
    public ItemGrantedFromGachaDomainEventHandler(
        IItemDefinitionRepository itemDefinitionRepository,
        INotificationRepository notificationRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _itemDefinitionRepository = itemDefinitionRepository;
        _notificationRepository = notificationRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ItemGrantedFromGachaDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var itemDefinition = await _itemDefinitionRepository.GetByIdAsync(domainEvent.ItemDefinitionId, cancellationToken);
        var nameVi = itemDefinition?.NameVi ?? domainEvent.ItemCode;
        var nameEn = itemDefinition?.NameEn ?? domainEvent.ItemCode;
        var nameZh = itemDefinition?.NameZh ?? domainEvent.ItemCode;

        await _notificationRepository.CreateAsync(
            new NotificationCreateDto
            {
                UserId = domainEvent.UserId,
                Type = GachaNotificationTypes.ItemGranted,
                TitleVi = "Nhận vật phẩm từ Gacha",
                TitleEn = "Item granted from Gacha",
                TitleZh = "抽卡获得道具",
                BodyVi = string.Format(
                    CultureInfo.InvariantCulture,
                    "Bạn nhận được {0} x{1} từ pool {2}.",
                    nameVi,
                    domainEvent.QuantityGranted,
                    domainEvent.PoolCode),
                BodyEn = string.Format(
                    CultureInfo.InvariantCulture,
                    "You received {0} x{1} from pool {2}.",
                    nameEn,
                    domainEvent.QuantityGranted,
                    domainEvent.PoolCode),
                BodyZh = string.Format(
                    CultureInfo.InvariantCulture,
                    "你从卡池 {2} 获得了 {0} x{1}。",
                    nameZh,
                    domainEvent.QuantityGranted,
                    domainEvent.PoolCode),
            },
            cancellationToken);
    }
}
