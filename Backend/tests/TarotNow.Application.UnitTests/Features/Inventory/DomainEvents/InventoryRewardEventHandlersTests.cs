using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
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
            SpreadCardCount = 5,
            SourceItemCode = InventoryItemCodes.FreeDrawTicket5,
        };

        var handler = new FreeDrawGrantedDomainEventHandler(
            freeDrawCreditRepositoryMock.Object,
            notificationRepositoryMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<FreeDrawGrantedDomainEvent>(domainEvent), CancellationToken.None);

        freeDrawCreditRepositoryMock.Verify(
            x => x.AddCreditsAsync(
                domainEvent.UserId,
                domainEvent.SpreadCardCount,
                domainEvent.GrantedCount,
                It.IsAny<CancellationToken>()),
            Times.Once);
        notificationRepositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<NotificationCreateDto>(n => n.Type == InventoryNotificationTypes.FreeDrawGranted),
                It.IsAny<CancellationToken>()),
            Times.Once);
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
                InventoryBusinessConstants.LuckyStarOwnedTitleGoldReward,
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
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<LuckyStarTitleUsedDomainEvent>(domainEvent), CancellationToken.None);

        walletRepositoryMock.Verify(
            x => x.CreditWithResultAsync(
                domainEvent.UserId,
                CurrencyType.Gold,
                TransactionType.InventoryReward,
                InventoryBusinessConstants.LuckyStarOwnedTitleGoldReward,
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
                    && e.DeltaAmount == InventoryBusinessConstants.LuckyStarOwnedTitleGoldReward),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
