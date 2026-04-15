using Moq;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;
using TarotNow.Domain.Events.Inventory;
using TarotNow.Domain.ValueObjects;

namespace TarotNow.Application.UnitTests.Features.Inventory.DomainEvents;

/// <summary>
/// Unit tests cho ItemUsedDomainEventHandler.
/// </summary>
public class ItemUsedDomainEventHandlerTests
{
    private readonly Mock<IItemDefinitionRepository> _itemDefinitionRepositoryMock = new();
    private readonly Mock<IUserItemRepository> _userItemRepositoryMock = new();
    private readonly Mock<IUserCollectionRepository> _userCollectionRepositoryMock = new();
    private readonly Mock<IInlineDomainEventDispatcher> _inlineDispatcherMock = new();
    private readonly Mock<IEventHandlerIdempotencyService> _idempotencyServiceMock = new();

    /// <summary>
    /// Xác nhận replay theo idempotency key sẽ dừng sớm, không dispatch effect mới.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldMarkReplayAndSkipEffect_WhenConsumeResultAlreadyProcessed()
    {
        var definition = BuildItemDefinition(
            code: InventoryItemCodes.FreeDrawTicket,
            itemType: ItemType.ReadingBooster,
            enhancementType: EnhancementType.FreeDraw);
        var domainEvent = new ItemUsedDomainEvent
        {
            UserId = Guid.NewGuid(),
            ItemCode = definition.Code,
            IdempotencyKey = "same-key",
        };

        _itemDefinitionRepositoryMock
            .Setup(x => x.GetByCodeAsync(definition.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(definition);
        _userItemRepositoryMock
            .Setup(x => x.TryConsumeWithIdempotencyAsync(It.IsAny<InventoryItemConsumeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(InventoryItemConsumeResult.AlreadyProcessed);

        var handler = BuildHandler();
        await handler.Handle(new DomainEventNotification<ItemUsedDomainEvent>(domainEvent), CancellationToken.None);

        Assert.True(domainEvent.IsIdempotentReplay);
        _inlineDispatcherMock.Verify(
            x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Xác nhận consume hết số lượng sẽ ném BusinessRuleException đúng error code.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldThrowBusinessRule_WhenConsumeResultOutOfStock()
    {
        var definition = BuildItemDefinition(
            code: InventoryItemCodes.FreeDrawTicket,
            itemType: ItemType.ReadingBooster,
            enhancementType: EnhancementType.FreeDraw);
        var domainEvent = new ItemUsedDomainEvent
        {
            UserId = Guid.NewGuid(),
            ItemCode = definition.Code,
            IdempotencyKey = "out-of-stock",
        };

        _itemDefinitionRepositoryMock
            .Setup(x => x.GetByCodeAsync(definition.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(definition);
        _userItemRepositoryMock
            .Setup(x => x.TryConsumeWithIdempotencyAsync(It.IsAny<InventoryItemConsumeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(InventoryItemConsumeResult.OutOfStock);

        var handler = BuildHandler();
        var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
            handler.Handle(new DomainEventNotification<ItemUsedDomainEvent>(domainEvent), CancellationToken.None));

        Assert.Equal(InventoryErrorCodes.ItemOutOfStock, exception.ErrorCode);
    }

    /// <summary>
    /// Xác nhận mystery card pack sẽ publish event mở pack để xử lý reward.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishMysteryPackEvent_WhenUsingMysteryCardPack()
    {
        var definition = BuildItemDefinition(
            code: InventoryItemCodes.MysteryCardPack,
            itemType: ItemType.ConsumableSpecial,
            enhancementType: null);
        var domainEvent = new ItemUsedDomainEvent
        {
            UserId = Guid.NewGuid(),
            ItemCode = definition.Code,
            IdempotencyKey = "mystery-open",
        };

        _itemDefinitionRepositoryMock
            .Setup(x => x.GetByCodeAsync(definition.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(definition);
        _userItemRepositoryMock
            .Setup(x => x.TryConsumeWithIdempotencyAsync(It.IsAny<InventoryItemConsumeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(InventoryItemConsumeResult.Consumed);
        _inlineDispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = BuildHandler();
        await handler.Handle(new DomainEventNotification<ItemUsedDomainEvent>(domainEvent), CancellationToken.None);

        _inlineDispatcherMock.Verify(
            x => x.PublishAsync(
                It.Is<MysteryPackOpenedDomainEvent>(e =>
                    e.UserId == domainEvent.UserId
                    && e.SourceItemCode == InventoryItemCodes.MysteryCardPack),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private ItemUsedDomainEventHandler BuildHandler()
    {
        return new ItemUsedDomainEventHandler(
            _itemDefinitionRepositoryMock.Object,
            _userItemRepositoryMock.Object,
            _userCollectionRepositoryMock.Object,
            _inlineDispatcherMock.Object,
            _idempotencyServiceMock.Object);
    }

    private static ItemDefinition BuildItemDefinition(string code, string itemType, string? enhancementType)
    {
        return new ItemDefinition(
            id: Guid.NewGuid(),
            code: code,
            type: itemType,
            rarity: ItemRarity.Common,
            isConsumable: true,
            isPermanent: false,
            effectValue: 1,
            successRatePercent: 100m,
            nameVi: "Tên",
            nameEn: "Name",
            nameZh: "名称",
            descriptionVi: "Mô tả",
            descriptionEn: "Description",
            descriptionZh: "描述",
            enhancementType: enhancementType,
            iconUrl: null,
            isActive: true);
    }
}
