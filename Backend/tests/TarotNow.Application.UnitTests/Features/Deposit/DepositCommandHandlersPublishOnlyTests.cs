using Moq;
using TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;
using TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Deposit;

// Unit tests đảm bảo command handler deposit chỉ publish domain event theo Rule 0.
public class DepositCommandHandlersPublishOnlyTests
{
    /// <summary>
    /// CreateDepositOrderCommandHandler phải chỉ publish event và trả response từ event data.
    /// </summary>
    [Fact]
    public async Task CreateDepositOrderCommandHandler_ShouldPublishDomainEvent_AndReturnHydratedResponse()
    {
        var dispatcher = new Mock<IInlineDomainEventDispatcher>();
        dispatcher
            .Setup(service => service.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback<IDomainEvent, CancellationToken>((domainEvent, _) =>
            {
                var depositEvent = Assert.IsType<DepositOrderCreateRequestedDomainEvent>(domainEvent);
                depositEvent.OrderId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                depositEvent.Status = "pending";
                depositEvent.AmountVnd = 100_000;
                depositEvent.BaseDiamondAmount = 1_000;
                depositEvent.BonusGoldAmount = 50;
                depositEvent.TotalDiamondAmount = 1_000;
                depositEvent.PayOsOrderCode = 990_001;
                depositEvent.PaymentLinkStatus = "ready";
                depositEvent.CheckoutUrl = "https://payos.test/checkout/990001";
                depositEvent.QrCode = "PAYOS_QR_990001";
                depositEvent.PaymentLinkId = "plink_990001";
                depositEvent.ExpiresAtUtc = DateTime.UtcNow.AddMinutes(15);
            })
            .Returns(Task.CompletedTask);

        var handler = new CreateDepositOrderCommandHandler(dispatcher.Object);
        var command = new CreateDepositOrderCommand
        {
            UserId = Guid.NewGuid(),
            PackageCode = "topup_100k",
            IdempotencyKey = "idempotency-key"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("pending", result.Status);
        Assert.Equal(100_000, result.AmountVnd);
        Assert.Equal(1_000, result.BaseDiamondAmount);
        Assert.Equal(50, result.BonusGoldAmount);
        Assert.Equal(990_001, result.PayOsOrderCode);
        Assert.Equal("ready", result.PaymentLinkStatus);
        dispatcher.Verify(
            service => service.PublishAsync(
                It.Is<DepositOrderCreateRequestedDomainEvent>(domainEvent =>
                    domainEvent.UserId == command.UserId
                    && domainEvent.PackageCode == command.PackageCode
                    && domainEvent.IdempotencyKey == command.IdempotencyKey),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// ProcessDepositWebhookCommandHandler phải chỉ publish event và phản ánh trạng thái Handled.
    /// </summary>
    [Fact]
    public async Task ProcessDepositWebhookCommandHandler_ShouldPublishDomainEvent_AndReturnHandledFlag()
    {
        var dispatcher = new Mock<IInlineDomainEventDispatcher>();
        dispatcher
            .Setup(service => service.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback<IDomainEvent, CancellationToken>((domainEvent, _) =>
            {
                var webhookEvent = Assert.IsType<DepositWebhookReceivedDomainEvent>(domainEvent);
                webhookEvent.Handled = true;
            })
            .Returns(Task.CompletedTask);

        var handler = new ProcessDepositWebhookCommandHandler(dispatcher.Object);
        var command = new ProcessDepositWebhookCommand
        {
            RawPayload = "{\"code\":\"00\"}"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        dispatcher.Verify(
            service => service.PublishAsync(
                It.Is<DepositWebhookReceivedDomainEvent>(domainEvent => domainEvent.RawPayload == command.RawPayload),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
