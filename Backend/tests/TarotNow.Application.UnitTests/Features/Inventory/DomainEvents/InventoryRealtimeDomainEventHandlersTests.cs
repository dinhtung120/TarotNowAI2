using Moq;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events.Gacha;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.UnitTests.Features.Inventory.DomainEvents;

/// <summary>
/// Unit tests xác nhận realtime handlers vẫn publish state sync sau khi tắt in-app notification.
/// </summary>
public class InventoryRealtimeDomainEventHandlersTests
{
    private readonly Mock<IEventHandlerIdempotencyService> _idempotencyServiceMock = new();

    [Fact]
    public async Task ItemGrantedFromGachaRealtimeHandler_ShouldPublishInventoryChanged()
    {
        var redisPublisherMock = new Mock<IRedisPublisher>();
        var domainEvent = new ItemGrantedFromGachaDomainEvent
        {
            UserId = Guid.NewGuid(),
            ItemDefinitionId = Guid.NewGuid(),
            ItemCode = "exp_booster",
            QuantityGranted = 2,
            PoolCode = "premium",
            PullOperationId = Guid.NewGuid(),
        };
        var handler = new ItemGrantedFromGachaRealtimeHandler(
            redisPublisherMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<ItemGrantedFromGachaDomainEvent>(domainEvent), CancellationToken.None);

        redisPublisherMock.Verify(
            x => x.PublishAsync(
                RealtimeChannelNames.UserState,
                RealtimeEventNames.InventoryChanged,
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CardEnhancedRealtimeHandler_ShouldPublishInventoryChanged()
    {
        var redisPublisherMock = new Mock<IRedisPublisher>();
        var domainEvent = new CardEnhancedDomainEvent
        {
            UserId = Guid.NewGuid(),
            CardId = 88,
            EnhancementType = "exp",
            ExpDelta = 12,
            AttackDelta = 0,
            DefenseDelta = 0,
            UpgradeSucceeded = false,
            SourceItemCode = "exp_booster",
        };
        var handler = new CardEnhancedRealtimeHandler(
            redisPublisherMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<CardEnhancedDomainEvent>(domainEvent), CancellationToken.None);

        redisPublisherMock.Verify(
            x => x.PublishAsync(
                RealtimeChannelNames.UserState,
                RealtimeEventNames.InventoryChanged,
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task FreeDrawGrantedRealtimeHandler_ShouldPublishReadingQuotaChanged()
    {
        var redisPublisherMock = new Mock<IRedisPublisher>();
        var domainEvent = new FreeDrawGrantedDomainEvent
        {
            UserId = Guid.NewGuid(),
            GrantedCount = 3,
            SpreadCardCount = 5,
            SourceItemCode = "free_draw_ticket_5",
        };
        var handler = new FreeDrawGrantedRealtimeHandler(
            redisPublisherMock.Object,
            _idempotencyServiceMock.Object);

        await handler.Handle(new DomainEventNotification<FreeDrawGrantedDomainEvent>(domainEvent), CancellationToken.None);

        redisPublisherMock.Verify(
            x => x.PublishAsync(
                RealtimeChannelNames.UserState,
                RealtimeEventNames.ReadingQuotaChanged,
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
