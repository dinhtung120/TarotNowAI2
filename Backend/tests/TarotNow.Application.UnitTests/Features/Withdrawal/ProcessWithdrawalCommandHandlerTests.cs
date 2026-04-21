using Moq;
using TarotNow.Application.Features.Withdrawal.Commands.ProcessWithdrawal;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Withdrawal;

/// <summary>
/// Unit tests đảm bảo process withdrawal command handler chỉ publish domain event theo Rule 0.
/// </summary>
public class ProcessWithdrawalCommandHandlerTests
{
    /// <summary>
    /// Handler phải publish <see cref="WithdrawalProcessRequestedDomainEvent"/> và trả trạng thái xử lý thành công.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldPublishDomainEvent_AndReturnTrue()
    {
        var dispatcher = new Mock<IInlineDomainEventDispatcher>();
        dispatcher
            .Setup(service => service.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new ProcessWithdrawalCommandHandler(dispatcher.Object);
        var command = new ProcessWithdrawalCommand
        {
            RequestId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            AdminId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            Action = "reject",
            AdminNote = "Thong tin ngan hang khong hop le",
            IdempotencyKey = "wd-process-001"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        dispatcher.Verify(
            service => service.PublishAsync(
                It.Is<WithdrawalProcessRequestedDomainEvent>(domainEvent =>
                    domainEvent.RequestId == command.RequestId
                    && domainEvent.AdminId == command.AdminId
                    && domainEvent.Action == command.Action
                    && domainEvent.AdminNote == command.AdminNote
                    && domainEvent.IdempotencyKey == command.IdempotencyKey),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
