using Moq;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.DomainEvents;

/// <summary>
/// Unit tests cho các handler xử lý EscrowSessionReleasedDomainEvent.
/// </summary>
public class EscrowSessionReleasedDomainEventHandlersTests
{
    [Fact]
    public async Task InAppNotificationHandler_ShouldCreateExactlyTwoNotifications_PerSession()
    {
        var notificationRepositoryMock = new Mock<INotificationRepository>();
        var redisPublisherMock = new Mock<IRedisPublisher>();
        var idempotencyServiceMock = new Mock<IEventHandlerIdempotencyService>();
        var domainEvent = new EscrowSessionReleasedDomainEvent
        {
            FinanceSessionId = Guid.NewGuid(),
            PayerId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            GrossAmountDiamond = 30,
            FeeAmountDiamond = 3,
            ReleasedAmountDiamond = 27,
            ReleasedItemCount = 2,
            IsAutoRelease = false
        };

        var handler = new EscrowSessionReleasedInAppNotificationHandler(
            notificationRepositoryMock.Object,
            redisPublisherMock.Object,
            idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<EscrowSessionReleasedDomainEvent>(domainEvent), CancellationToken.None);

        notificationRepositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<NotificationCreateDto>(n =>
                    n.UserId == domainEvent.PayerId
                    && n.Type == "escrow_released"
                    && n.Metadata != null
                    && n.Metadata.ContainsKey("financeSessionId")),
                It.IsAny<CancellationToken>()),
            Times.Once);
        notificationRepositoryMock.Verify(
            x => x.CreateAsync(
                It.Is<NotificationCreateDto>(n =>
                    n.UserId == domainEvent.ReceiverId
                    && n.Type == "escrow_income"
                    && n.Metadata != null
                    && n.Metadata.ContainsKey("financeSessionId")),
                It.IsAny<CancellationToken>()),
            Times.Once);
        redisPublisherMock.Verify(
            x => x.PublishAsync(
                RealtimeChannelNames.Notifications,
                RealtimeEventNames.NotificationNew,
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task MoneyChangedHandler_ShouldPublishReceiverDeltaAndPayerSnapshot()
    {
        var domainEventPublisherMock = new Mock<IDomainEventPublisher>();
        var idempotencyServiceMock = new Mock<IEventHandlerIdempotencyService>();
        var domainEvent = new EscrowSessionReleasedDomainEvent
        {
            FinanceSessionId = Guid.NewGuid(),
            PayerId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            GrossAmountDiamond = 100,
            FeeAmountDiamond = 10,
            ReleasedAmountDiamond = 90,
            ReleasedItemCount = 2,
            IsAutoRelease = true
        };

        var handler = new EscrowSessionReleasedMoneyChangedDomainEventHandler(
            domainEventPublisherMock.Object,
            idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<EscrowSessionReleasedDomainEvent>(domainEvent), CancellationToken.None);

        domainEventPublisherMock.Verify(
            x => x.PublishAsync(
                It.Is<MoneyChangedDomainEvent>(e =>
                    e.UserId == domainEvent.ReceiverId
                    && e.Currency == CurrencyType.Diamond
                    && e.ChangeType == TransactionType.EscrowRelease
                    && e.DeltaAmount == domainEvent.ReleasedAmountDiamond
                    && e.ReferenceId == domainEvent.FinanceSessionId.ToString()),
                It.IsAny<CancellationToken>()),
            Times.Once);
        domainEventPublisherMock.Verify(
            x => x.PublishAsync(
                It.Is<WalletSnapshotChangedDomainEvent>(e =>
                    e.UserId == domainEvent.PayerId
                    && e.Currency == CurrencyType.Diamond
                    && e.ChangeType == TransactionType.EscrowRelease
                    && e.ReferenceId == domainEvent.FinanceSessionId.ToString()),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
