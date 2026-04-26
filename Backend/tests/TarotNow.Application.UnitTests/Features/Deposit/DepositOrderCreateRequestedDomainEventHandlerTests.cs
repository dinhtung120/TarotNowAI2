using Moq;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.DomainEvents.Handlers;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.UnitTests.Features.Deposit;

public sealed class DepositOrderCreateRequestedDomainEventHandlerTests
{
    [Fact]
    public async Task Handle_ShouldEnqueueProvisionEvent_WhenOrderHasNoPaymentLink()
    {
        var orderRepository = new Mock<IDepositOrderRepository>();
        var promotionRepository = new Mock<IDepositPromotionRepository>();
        var packageCatalog = new Mock<IDepositPackageCatalog>();
        var domainEventPublisher = new Mock<IDomainEventPublisher>();
        var idempotency = new Mock<IEventHandlerIdempotencyService>();

        idempotency
            .Setup(service => service.HasProcessedInlineEventAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        idempotency
            .Setup(service => service.MarkInlineEventProcessedAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var packageDefinition = new DepositPackageDefinition(
            Code: "topup_100k",
            AmountVnd: 100_000,
            BaseDiamond: 1_000,
            IsActive: true);

        packageCatalog
            .Setup(catalog => catalog.FindByCode("topup_100k"))
            .Returns(packageDefinition);
        promotionRepository
            .Setup(repo => repo.GetActivePromotionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<DepositPromotion>());
        orderRepository
            .Setup(repo => repo.AcquireCreateOrderLockAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        orderRepository
            .Setup(repo => repo.GetByClientRequestKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((DepositOrder?)null);
        orderRepository
            .Setup(repo => repo.GetNextPayOsOrderCodeAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(12345678901L);
        orderRepository
            .Setup(repo => repo.AddOrGetExistingByClientRequestKeyAsync(
                It.IsAny<DepositOrder>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((DepositOrder order, string _, CancellationToken _) => order);

        var handler = new DepositOrderCreateRequestedDomainEventHandler(
            orderRepository.Object,
            promotionRepository.Object,
            packageCatalog.Object,
            domainEventPublisher.Object,
            idempotency.Object);

        var createEvent = new DepositOrderCreateRequestedDomainEvent
        {
            UserId = Guid.NewGuid(),
            PackageCode = "topup_100k",
            IdempotencyKey = "create-request-key"
        };

        await handler.Handle(
            new DomainEventNotification<DepositOrderCreateRequestedDomainEvent>(
                createEvent,
                OutboxMessageId: null,
                EventIdempotencyKey: createEvent.EventIdempotencyKey),
            CancellationToken.None);

        Assert.Equal("provisioning", createEvent.PaymentLinkStatus);
        Assert.True(string.IsNullOrWhiteSpace(createEvent.PaymentLinkId));
        domainEventPublisher.Verify(
            publisher => publisher.PublishAsync(
                It.Is<DepositPaymentLinkProvisionRequestedDomainEvent>(domainEvent =>
                    domainEvent.OrderId == createEvent.OrderId
                    && domainEvent.PayOsOrderCode == createEvent.PayOsOrderCode),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotEnqueueProvisionEvent_WhenOrderAlreadyHasPaymentLink()
    {
        var orderRepository = new Mock<IDepositOrderRepository>();
        var promotionRepository = new Mock<IDepositPromotionRepository>();
        var packageCatalog = new Mock<IDepositPackageCatalog>();
        var domainEventPublisher = new Mock<IDomainEventPublisher>();
        var idempotency = new Mock<IEventHandlerIdempotencyService>();

        idempotency
            .Setup(service => service.HasProcessedInlineEventAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        idempotency
            .Setup(service => service.MarkInlineEventProcessedAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var userId = Guid.NewGuid();
        var existingOrder = new DepositOrder(
            userId,
            packageCode: "topup_100k",
            amountVnd: 100_000,
            baseDiamondAmount: 1_000,
            bonusGoldAmount: 0,
            clientRequestKey: "fixed-key",
            payOsOrderCode: 99887766,
            payOsPaymentLinkId: "plink_existing",
            checkoutUrl: "https://payos.test/checkout",
            qrCode: "QR",
            expiresAtUtc: DateTime.UtcNow.AddMinutes(15));

        orderRepository
            .Setup(repo => repo.AcquireCreateOrderLockAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        orderRepository
            .Setup(repo => repo.GetByClientRequestKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingOrder);

        var handler = new DepositOrderCreateRequestedDomainEventHandler(
            orderRepository.Object,
            promotionRepository.Object,
            packageCatalog.Object,
            domainEventPublisher.Object,
            idempotency.Object);

        var createEvent = new DepositOrderCreateRequestedDomainEvent
        {
            UserId = userId,
            PackageCode = "topup_100k",
            IdempotencyKey = "dup-key"
        };

        await handler.Handle(
            new DomainEventNotification<DepositOrderCreateRequestedDomainEvent>(
                createEvent,
                OutboxMessageId: null,
                EventIdempotencyKey: createEvent.EventIdempotencyKey),
            CancellationToken.None);

        Assert.Equal("ready", createEvent.PaymentLinkStatus);
        Assert.Equal("plink_existing", createEvent.PaymentLinkId);
        domainEventPublisher.Verify(
            publisher => publisher.PublishAsync(
                It.IsAny<DepositPaymentLinkProvisionRequestedDomainEvent>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
