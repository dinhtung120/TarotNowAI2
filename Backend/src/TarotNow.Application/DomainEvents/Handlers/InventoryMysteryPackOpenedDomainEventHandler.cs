using System.Globalization;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler xử lý reward khi người dùng mở mystery card pack.
/// </summary>
public sealed class MysteryPackOpenedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MysteryPackOpenedDomainEvent>
{
    private static readonly MysteryPackReward[] RewardPool =
    [
        new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65011"), InventoryItemCodes.ExpBooster, 1, 3000),
        new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65022"), InventoryItemCodes.PowerBooster, 1, 2200),
        new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65033"), InventoryItemCodes.DefenseBooster, 1, 2200),
        new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65055"), InventoryItemCodes.FreeDrawTicket3, 1, 1200),
        new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65066"), InventoryItemCodes.FreeDrawTicket5, 1, 900),
        new(new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65077"), InventoryItemCodes.FreeDrawTicket10, 1, 500),
    ];

    private readonly IRngService _rngService;
    private readonly IUserItemRepository _userItemRepository;
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Khởi tạo handler mystery pack.
    /// </summary>
    public MysteryPackOpenedDomainEventHandler(
        IRngService rngService,
        IUserItemRepository userItemRepository,
        INotificationRepository notificationRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _rngService = rngService;
        _userItemRepository = userItemRepository;
        _notificationRepository = notificationRepository;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        MysteryPackOpenedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var reward = ResolveReward();
        await _userItemRepository.GrantItemByCodeAsync(
            domainEvent.UserId,
            reward.ItemCode,
            reward.Quantity,
            cancellationToken);
        await _notificationRepository.CreateAsync(
            new NotificationCreateDto
            {
                UserId = domainEvent.UserId,
                Type = InventoryNotificationTypes.MysteryPackOpened,
                TitleVi = InventoryNotificationTemplates.MysteryPackOpenedTitleVi,
                TitleEn = InventoryNotificationTemplates.MysteryPackOpenedTitleEn,
                TitleZh = InventoryNotificationTemplates.MysteryPackOpenedTitleZh,
                BodyVi = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.MysteryPackOpenedBodyVi,
                    reward.ItemCode,
                    reward.Quantity,
                    domainEvent.SourceItemCode),
                BodyEn = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.MysteryPackOpenedBodyEn,
                    reward.ItemCode,
                    reward.Quantity,
                    domainEvent.SourceItemCode),
                BodyZh = string.Format(
                    CultureInfo.InvariantCulture,
                    InventoryNotificationTemplates.MysteryPackOpenedBodyZh,
                    reward.ItemCode,
                    reward.Quantity,
                    domainEvent.SourceItemCode),
            },
            cancellationToken);
    }

    private MysteryPackReward ResolveReward()
    {
        var weightedItems = RewardPool.Select(
            reward => new WeightedItem
            {
                ItemId = reward.Id,
                WeightBasisPoints = reward.WeightBasisPoints,
            });

        var rngResult = _rngService.WeightedSelect(weightedItems);
        var reward = RewardPool.FirstOrDefault(x => x.Id == rngResult.SelectedItemId);
        if (reward is null)
        {
            throw new InvalidOperationException("Mystery pack reward could not be resolved.");
        }

        return reward;
    }

    private sealed record MysteryPackReward(Guid Id, string ItemCode, int Quantity, int WeightBasisPoints);
}
