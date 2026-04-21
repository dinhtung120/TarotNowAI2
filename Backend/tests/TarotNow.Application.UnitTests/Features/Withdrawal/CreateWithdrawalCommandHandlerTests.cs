using Moq;
using TarotNow.Application.Features.Withdrawal.Commands.CreateWithdrawal;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Withdrawal;

/// <summary>
/// Unit tests đảm bảo create withdrawal command handler chỉ publish domain event theo Rule 0.
/// </summary>
public class CreateWithdrawalCommandHandlerTests
{
    /// <summary>
    /// Handler phải publish <see cref="WithdrawalCreateRequestedDomainEvent"/> và trả request id được hydrate từ event handler.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishDomainEvent_AndReturnHydratedRequestId()
    {
        var expectedRequestId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var dispatcher = new Mock<IInlineDomainEventDispatcher>();
        dispatcher
            .Setup(service => service.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback<IDomainEvent, CancellationToken>((domainEvent, _) =>
            {
                var createEvent = Assert.IsType<WithdrawalCreateRequestedDomainEvent>(domainEvent);
                createEvent.RequestId = expectedRequestId;
                createEvent.Status = "pending";
            })
            .Returns(Task.CompletedTask);

        var handler = new CreateWithdrawalCommandHandler(dispatcher.Object);
        var command = new CreateWithdrawalCommand
        {
            UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            AmountDiamond = 500,
            IdempotencyKey = "wd-create-001",
            UserNote = "Rut tuan nay"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedRequestId, result);
        dispatcher.Verify(
            service => service.PublishAsync(
                It.Is<WithdrawalCreateRequestedDomainEvent>(domainEvent =>
                    domainEvent.UserId == command.UserId
                    && domainEvent.AmountDiamond == command.AmountDiamond
                    && domainEvent.IdempotencyKey == command.IdempotencyKey
                    && domainEvent.UserNote == command.UserNote),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
