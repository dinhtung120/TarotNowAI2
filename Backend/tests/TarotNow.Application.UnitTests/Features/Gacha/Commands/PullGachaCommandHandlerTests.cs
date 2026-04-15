using Moq;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Gacha.Commands.PullGacha;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Domain.Events.Gacha;

namespace TarotNow.Application.UnitTests.Features.Gacha.Commands;

/// <summary>
/// Unit tests cho <see cref="PullGachaCommandHandler"/>.
/// </summary>
public class PullGachaCommandHandlerTests
{
    private readonly Mock<IInlineDomainEventDispatcher> _inlineDispatcherMock = new();

    /// <summary>
    /// Xác nhận handler chỉ publish event và map snapshot kết quả về response.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishEventAndReturnMappedResult_WhenRequestIsValid()
    {
        var publishedEvents = new List<GachaPulledDomainEvent>();
        _inlineDispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns<IDomainEvent, CancellationToken>((domainEvent, _) =>
            {
                var typedEvent = (GachaPulledDomainEvent)domainEvent;
                typedEvent.CurrentPityCount = 12;
                typedEvent.HardPityThreshold = 70;
                typedEvent.WasPityTriggered = false;
                typedEvent.Rewards =
                [
                    new GachaPullRewardSnapshot
                    {
                        Kind = GachaRewardTypes.Item,
                        Rarity = "epic",
                        ItemCode = "exp_booster",
                        QuantityGranted = 1,
                        NameVi = "Bình EXP",
                        NameEn = "EXP Booster",
                        NameZh = "经验强化剂",
                    },
                ];
                publishedEvents.Add(typedEvent);
                return Task.CompletedTask;
            });

        var handler = new PullGachaCommandHandler(_inlineDispatcherMock.Object);
        var request = new PullGachaCommand
        {
            UserId = Guid.NewGuid(),
            PoolCode = " Premium ",
            Count = 1,
            IdempotencyKey = " key-001 ",
        };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Single(publishedEvents);
        Assert.Equal("premium", publishedEvents[0].PoolCode);
        Assert.Equal("key-001", publishedEvents[0].IdempotencyKey);
        Assert.True(result.Success);
        Assert.Equal("premium", result.PoolCode);
        Assert.Equal(12, result.CurrentPityCount);
        Assert.Single(result.Rewards);
        Assert.Equal("exp_booster", result.Rewards[0].ItemCode);
    }

    /// <summary>
    /// Xác nhận handler chặn request thiếu idempotency key.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowBusinessRuleException_WhenIdempotencyKeyIsMissing()
    {
        var handler = new PullGachaCommandHandler(_inlineDispatcherMock.Object);
        var request = new PullGachaCommand
        {
            UserId = Guid.NewGuid(),
            PoolCode = "normal",
            Count = 1,
            IdempotencyKey = " ",
        };

        var action = async () => await handler.Handle(request, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<BusinessRuleException>(action);
        Assert.Equal(GachaErrorCodes.InvalidIdempotencyKey, exception.ErrorCode);
    }
}
