using Moq;
using System.Collections.Generic;
using System.Reflection;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;
using TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

/// <summary>
/// Unit tests xác nhận các flow complete conversation gọi session-level settlement đúng một lần.
/// </summary>
public class ConversationCompleteSessionSettlementTests
{
    [Fact]
    public async Task RequestCompleteSettlement_ShouldCallSessionReleaseOnce()
    {
        var financeRepo = new Mock<IChatFinanceRepository>();
        var escrowSettlementService = new Mock<IEscrowSettlementService>();
        var session = new ChatFinanceSession
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ReaderId = Guid.NewGuid(),
            TotalFrozen = 30,
            Status = ChatFinanceSessionStatus.Active
        };
        var items = new List<ChatQuestionItem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FinanceSessionId = session.Id,
                PayerId = session.UserId,
                ReceiverId = session.ReaderId,
                Status = QuestionItemStatus.Accepted,
                AmountDiamond = 30
            }
        };

        financeRepo.Setup(x => x.GetSessionByConversationRefAsync("conv-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);
        financeRepo.Setup(x => x.GetItemsBySessionIdAsync(session.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);
        escrowSettlementService.Setup(x => x.ApplySessionReleaseAsync(session, items, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EscrowSessionReleaseSummary
            {
                FinanceSessionId = session.Id,
                PayerId = session.UserId,
                ReceiverId = session.ReaderId,
                GrossAmountDiamond = 30,
                FeeAmountDiamond = 3,
                ReleasedAmountDiamond = 27,
                ReleasedItemCount = 1,
                IsAutoRelease = false
            });

        var handler = new RequestConversationCompleteCommandHandlerRequestedDomainEventHandler(
            Mock.Of<IConversationRepository>(),
            financeRepo.Object,
            escrowSettlementService.Object,
            Mock.Of<ITransactionCoordinator>(),
            Mock.Of<IChatMessageRepository>(),
            Mock.Of<IDomainEventPublisher>(),
            Mock.Of<IChatRealtimeFastLanePublisher>(),
            Mock.Of<IEventHandlerIdempotencyService>());

        await InvokePrivateSettlementAsync(
            handler,
            "SettleConversationSessionAsync",
            "conv-1");

        escrowSettlementService.Verify(
            x => x.ApplySessionReleaseAsync(session, items, false, It.IsAny<CancellationToken>()),
            Times.Once);
        financeRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RespondCompleteSettlement_ShouldCallSessionReleaseOnce()
    {
        var financeRepo = new Mock<IChatFinanceRepository>();
        var escrowSettlementService = new Mock<IEscrowSettlementService>();
        var session = new ChatFinanceSession
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ReaderId = Guid.NewGuid(),
            TotalFrozen = 50,
            Status = ChatFinanceSessionStatus.Active
        };
        var items = new List<ChatQuestionItem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FinanceSessionId = session.Id,
                PayerId = session.UserId,
                ReceiverId = session.ReaderId,
                Status = QuestionItemStatus.Accepted,
                AmountDiamond = 50
            }
        };

        financeRepo.Setup(x => x.GetSessionByConversationRefAsync("conv-2", It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);
        financeRepo.Setup(x => x.GetItemsBySessionIdAsync(session.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);
        escrowSettlementService.Setup(x => x.ApplySessionReleaseAsync(session, items, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EscrowSessionReleaseSummary
            {
                FinanceSessionId = session.Id,
                PayerId = session.UserId,
                ReceiverId = session.ReaderId,
                GrossAmountDiamond = 50,
                FeeAmountDiamond = 5,
                ReleasedAmountDiamond = 45,
                ReleasedItemCount = 1,
                IsAutoRelease = false
            });

        var handler = new RespondConversationCompleteCommandHandlerRequestedDomainEventHandler(
            Mock.Of<IConversationRepository>(),
            financeRepo.Object,
            escrowSettlementService.Object,
            Mock.Of<ITransactionCoordinator>(),
            Mock.Of<IChatMessageRepository>(),
            Mock.Of<IDomainEventPublisher>(),
            Mock.Of<IChatRealtimeFastLanePublisher>(),
            Mock.Of<IEventHandlerIdempotencyService>());

        await InvokePrivateSettlementAsync(
            handler,
            "SettleConversationSessionAsync",
            "conv-2");

        escrowSettlementService.Verify(
            x => x.ApplySessionReleaseAsync(session, items, false, It.IsAny<CancellationToken>()),
            Times.Once);
        financeRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static async Task InvokePrivateSettlementAsync(
        object target,
        string methodName,
        string conversationId)
    {
        var method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(method);
        var task = (Task?)method!.Invoke(target, new object[] { conversationId, CancellationToken.None });
        Assert.NotNull(task);
        await task!;
    }
}
