using Moq;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Features.Inventory.Commands;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Domain.Events.Inventory;

namespace TarotNow.Application.UnitTests.Features.Inventory.Commands;

/// <summary>
/// Unit tests cho UseInventoryItemCommandHandler.
/// </summary>
public class UseInventoryItemCommandHandlerTests
{
    private readonly Mock<IInlineDomainEventDispatcher> _inlineDispatcherMock = new();

    /// <summary>
    /// Xác nhận command hợp lệ publish event và trả accepted.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishEventAndReturnAccepted_WhenRequestIsValid()
    {
        var publishedEvents = new List<ItemUsedDomainEvent>();
        _inlineDispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns<IDomainEvent, CancellationToken>((domainEvent, _) =>
            {
                publishedEvents.Add((ItemUsedDomainEvent)domainEvent);
                return Task.CompletedTask;
            });

        var handler = new UseInventoryItemCommandHandler(_inlineDispatcherMock.Object);
        var request = new UseInventoryItemCommand
        {
            UserId = Guid.NewGuid(),
            ItemCode = " EXP_BOOSTER ",
            TargetCardId = 19,
            IdempotencyKey = " replay-key ",
        };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Single(publishedEvents);
        Assert.Equal("exp_booster", publishedEvents[0].ItemCode);
        Assert.Equal("replay-key", publishedEvents[0].IdempotencyKey);
        Assert.False(result.IsIdempotentReplay);
        Assert.Equal(InventoryCommandMessages.Accepted, result.Message);
    }

    /// <summary>
    /// Xác nhận event replay được phản ánh đúng vào response command.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnReplayResult_WhenEventMarkedAsReplay()
    {
        _inlineDispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns<IDomainEvent, CancellationToken>((domainEvent, _) =>
            {
                ((ItemUsedDomainEvent)domainEvent).IsIdempotentReplay = true;
                return Task.CompletedTask;
            });

        var handler = new UseInventoryItemCommandHandler(_inlineDispatcherMock.Object);
        var request = new UseInventoryItemCommand
        {
            UserId = Guid.NewGuid(),
            ItemCode = "free_draw_ticket",
            IdempotencyKey = "same-key",
        };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.IsIdempotentReplay);
        Assert.Equal(InventoryCommandMessages.Replayed, result.Message);
    }
}
