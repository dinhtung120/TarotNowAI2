using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using TarotNow.Domain.Events.Gacha;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.UnitTests.Features.Inventory.DomainEvents;

/// <summary>
/// Unit tests cho các handler reward side-effects của inventory.
/// </summary>
public class InventoryRewardEventHandlersTests
{
    private readonly Mock<IEventHandlerIdempotencyService> _idempotencyServiceMock = new();

    /// <summary>
    /// Xác nhận handler free draw chỉ cộng credit và không tạo in-app notification.
    /// </summary>
    [Fact]
    public async Task FreeDrawGrantedHandler_ShouldPersistCreditWithoutCreatingNotification()
    {
        var freeDrawCreditRepositoryMock = new Mock<IFreeDrawCreditRepository>();
        var domainEvent = new FreeDrawGrantedDomainEvent
        {
            UserId = Guid.NewGuid(),
            GrantedCount = 2,
            SpreadCardCount = 5,
            SourceItemCode = InventoryItemCodes.FreeDrawTicket5,
        };

        var handler = new FreeDrawGrantedDomainEventHandler(
            freeDrawCreditRepositoryMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<FreeDrawGrantedDomainEvent>(domainEvent), CancellationToken.None);

        freeDrawCreditRepositoryMock.Verify(
            x => x.AddCreditsAsync(
                domainEvent.UserId,
                domainEvent.SpreadCardCount,
                domainEvent.GrantedCount,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Xác nhận handler ItemGrantedFromGacha đã tắt side-effect tạo in-app notification.
    /// </summary>
    [Fact]
    public async Task ItemGrantedFromGachaHandler_ShouldCompleteWithoutInAppNotificationSideEffects()
    {
        var domainEvent = new ItemGrantedFromGachaDomainEvent
        {
            UserId = Guid.NewGuid(),
            ItemDefinitionId = Guid.NewGuid(),
            ItemCode = InventoryItemCodes.ExpBooster,
            QuantityGranted = 1,
            PoolCode = "premium",
            PullOperationId = Guid.NewGuid(),
        };
        var handler = new ItemGrantedFromGachaDomainEventHandler(_idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<ItemGrantedFromGachaDomainEvent>(domainEvent), CancellationToken.None);
    }

    /// <summary>
    /// Xác nhận handler CardEnhanced đã tắt side-effect tạo in-app notification.
    /// </summary>
    [Fact]
    public async Task CardEnhancedHandler_ShouldCompleteWithoutInAppNotificationSideEffects()
    {
        var domainEvent = new CardEnhancedDomainEvent
        {
            UserId = Guid.NewGuid(),
            CardId = 101,
            EnhancementType = "exp",
            ExpDelta = 10,
            AttackDelta = 0,
            DefenseDelta = 0,
            UpgradeSucceeded = false,
            SourceItemCode = InventoryItemCodes.ExpBooster,
        };
        var handler = new CardEnhancedDomainEventHandler(_idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<CardEnhancedDomainEvent>(domainEvent), CancellationToken.None);
    }

    /// <summary>
    /// Xác nhận Lucky Star chưa sở hữu title sẽ publish TitleGrantedDomainEvent.
    /// </summary>
    [Fact]
    public async Task LuckyStarHandler_ShouldPublishTitleGranted_WhenUserDoesNotOwnTitle()
    {
        var titleRepositoryMock = new Mock<ITitleRepository>();
        var walletRepositoryMock = new Mock<IWalletRepository>();
        var domainEventPublisherMock = new Mock<IDomainEventPublisher>();
        var systemConfigSettingsMock = new Mock<ISystemConfigSettings>();
        systemConfigSettingsMock.SetupGet(x => x.InventoryLuckyStarOwnedTitleGoldReward).Returns(500);
        var domainEvent = new LuckyStarTitleUsedDomainEvent
        {
            UserId = Guid.NewGuid(),
            SourceItemCode = InventoryItemCodes.RareTitleLuckyStar,
        };

        titleRepositoryMock
            .Setup(x => x.OwnsTitleAsync(domainEvent.UserId, InventoryBusinessConstants.LuckyStarTitleCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new LuckyStarTitleUsedDomainEventHandler(
            titleRepositoryMock.Object,
            walletRepositoryMock.Object,
            domainEventPublisherMock.Object,
            systemConfigSettingsMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<LuckyStarTitleUsedDomainEvent>(domainEvent), CancellationToken.None);

        domainEventPublisherMock.Verify(
            x => x.PublishAsync(
                It.Is<TitleGrantedDomainEvent>(e =>
                    e.UserId == domainEvent.UserId
                    && e.TitleCode == InventoryBusinessConstants.LuckyStarTitleCode
                    && e.Source == domainEvent.SourceItemCode),
                It.IsAny<CancellationToken>()),
            Times.Once);
        walletRepositoryMock.Verify(
            x => x.CreditWithResultAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<long>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Xác nhận Lucky Star đã có title sẽ credit vàng và publish MoneyChangedDomainEvent.
    /// </summary>
    [Fact]
    public async Task LuckyStarHandler_ShouldCreditGold_WhenUserAlreadyOwnsTitle()
    {
        var titleRepositoryMock = new Mock<ITitleRepository>();
        var walletRepositoryMock = new Mock<IWalletRepository>();
        var domainEventPublisherMock = new Mock<IDomainEventPublisher>();
        var systemConfigSettingsMock = new Mock<ISystemConfigSettings>();
        const long expectedReward = 500;
        systemConfigSettingsMock.SetupGet(x => x.InventoryLuckyStarOwnedTitleGoldReward).Returns(expectedReward);
        var domainEvent = new LuckyStarTitleUsedDomainEvent
        {
            UserId = Guid.NewGuid(),
            SourceItemCode = InventoryItemCodes.RareTitleLuckyStar,
        };

        titleRepositoryMock
            .Setup(x => x.OwnsTitleAsync(domainEvent.UserId, InventoryBusinessConstants.LuckyStarTitleCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        walletRepositoryMock
            .Setup(x => x.CreditWithResultAsync(
                domainEvent.UserId,
                CurrencyType.Gold,
                TransactionType.InventoryReward,
                expectedReward,
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(WalletOperationResult.ExecutedResult);

        var handler = new LuckyStarTitleUsedDomainEventHandler(
            titleRepositoryMock.Object,
            walletRepositoryMock.Object,
            domainEventPublisherMock.Object,
            systemConfigSettingsMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<LuckyStarTitleUsedDomainEvent>(domainEvent), CancellationToken.None);

        walletRepositoryMock.Verify(
            x => x.CreditWithResultAsync(
                domainEvent.UserId,
                CurrencyType.Gold,
                TransactionType.InventoryReward,
                expectedReward,
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        domainEventPublisherMock.Verify(
            x => x.PublishAsync(
                It.Is<MoneyChangedDomainEvent>(e =>
                    e.UserId == domainEvent.UserId
                    && e.Currency == CurrencyType.Gold
                    && e.ChangeType == TransactionType.InventoryReward
                    && e.DeltaAmount == expectedReward),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
