using Moq;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.UnitTests.Features.Inventory.DomainEvents;

/// <summary>
/// Unit tests cho các handler reward side-effects của inventory.
/// </summary>
public class InventoryRewardEventHandlersTests
{
    private readonly Mock<IEventHandlerIdempotencyService> _idempotencyServiceMock = new();

    /// <summary>
    /// Xác nhận handler free draw sẽ cộng credit thật trước khi gửi notification.
    /// </summary>
    [Fact]
    public async Task FreeDrawGrantedHandler_ShouldPersistCreditAndCreateNotification()
    {
        var freeDrawCreditRepositoryMock = new Mock<IFreeDrawCreditRepository>();
        var notificationRepositoryMock = new Mock<INotificationRepository>();
        var domainEvent = new FreeDrawGrantedDomainEvent
        {
            UserId = Guid.NewGuid(),
            GrantedCount = 2,
            SourceItemCode = InventoryItemCodes.FreeDrawTicket,
        };

        var handler = new FreeDrawGrantedDomainEventHandler(
            freeDrawCreditRepositoryMock.Object,
            notificationRepositoryMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<FreeDrawGrantedDomainEvent>(domainEvent), CancellationToken.None);

        freeDrawCreditRepositoryMock.Verify(
            x => x.AddCreditsAsync(domainEvent.UserId, domainEvent.GrantedCount, It.IsAny<CancellationToken>()),
            Times.Once);
        notificationRepositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<NotificationCreateDto>(n => n.Type == InventoryNotificationTypes.FreeDrawGranted),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Xác nhận handler luck sẽ persist trạng thái hiệu ứng trước khi gửi notification.
    /// </summary>
    [Fact]
    public async Task LuckAppliedHandler_ShouldPersistLuckEffectAndCreateNotification()
    {
        var luckRepositoryMock = new Mock<IInventoryLuckEffectRepository>();
        var notificationRepositoryMock = new Mock<INotificationRepository>();
        var domainEvent = new LuckAppliedDomainEvent
        {
            UserId = Guid.NewGuid(),
            LuckValue = 10,
            SourceItemCode = InventoryItemCodes.DailyFortuneScroll,
        };

        var handler = new LuckAppliedDomainEventHandler(
            luckRepositoryMock.Object,
            notificationRepositoryMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<LuckAppliedDomainEvent>(domainEvent), CancellationToken.None);

        luckRepositoryMock.Verify(
            x => x.ApplyLuckAsync(
                domainEvent.UserId,
                domainEvent.LuckValue,
                domainEvent.SourceItemCode,
                It.Is<TimeSpan>(duration => duration > TimeSpan.Zero),
                It.IsAny<CancellationToken>()),
            Times.Once);
        notificationRepositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<NotificationCreateDto>(n => n.Type == InventoryNotificationTypes.LuckApplied),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Xác nhận mystery pack handler cấp đúng reward được RNG chọn và gửi notification.
    /// </summary>
    [Fact]
    public async Task MysteryPackOpenedHandler_ShouldGrantRewardAndNotify()
    {
        var rngServiceMock = new Mock<IRngService>();
        var userItemRepositoryMock = new Mock<IUserItemRepository>();
        var notificationRepositoryMock = new Mock<INotificationRepository>();
        var selectedRewardId = new Guid("0aa0f7b7-7c01-4b58-96d8-690cb1f65011");

        rngServiceMock
            .Setup(x => x.WeightedSelect(It.IsAny<IEnumerable<WeightedItem>>(), It.IsAny<string?>()))
            .Returns(new GachaRngResult
            {
                SelectedItemId = selectedRewardId,
                RngSeed = "seed",
            });

        var domainEvent = new MysteryPackOpenedDomainEvent
        {
            UserId = Guid.NewGuid(),
            SourceItemCode = InventoryItemCodes.MysteryCardPack,
        };

        var handler = new MysteryPackOpenedDomainEventHandler(
            rngServiceMock.Object,
            userItemRepositoryMock.Object,
            notificationRepositoryMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<MysteryPackOpenedDomainEvent>(domainEvent), CancellationToken.None);

        userItemRepositoryMock.Verify(
            x => x.GrantItemByCodeAsync(
                domainEvent.UserId,
                InventoryItemCodes.ExpBooster,
                1,
                It.IsAny<CancellationToken>()),
            Times.Once);
        notificationRepositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<NotificationCreateDto>(n => n.Type == InventoryNotificationTypes.MysteryPackOpened),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
