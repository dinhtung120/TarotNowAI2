using Moq;
using System.Collections.Generic;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Services;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Services;

/// <summary>
/// Unit tests cho nhánh settlement gộp theo finance session.
/// </summary>
public class EscrowSettlementServiceSessionTests
{
    private readonly Mock<IChatFinanceRepository> _financeRepositoryMock = new();
    private readonly Mock<IWalletRepository> _walletRepositoryMock = new();
    private readonly Mock<IDomainEventPublisher> _domainEventPublisherMock = new();
    private readonly Mock<ISystemConfigSettings> _systemConfigSettingsMock = new();

    [Fact]
    public async Task ApplySessionReleaseAsync_ShouldReleaseOnceAndPublishSessionEvent()
    {
        _systemConfigSettingsMock.SetupGet(x => x.WithdrawalFeeRate).Returns(0.10m);
        _systemConfigSettingsMock.SetupGet(x => x.EscrowDisputeWindowHours).Returns(48);

        var service = new EscrowSettlementService(
            _financeRepositoryMock.Object,
            _walletRepositoryMock.Object,
            _domainEventPublisherMock.Object,
            _systemConfigSettingsMock.Object);

        var session = new ChatFinanceSession
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ReaderId = Guid.NewGuid(),
            TotalFrozen = 30,
            Status = ChatFinanceSessionStatus.Active
        };
        var item1 = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            FinanceSessionId = session.Id,
            PayerId = session.UserId,
            ReceiverId = session.ReaderId,
            AmountDiamond = 10,
            Status = QuestionItemStatus.Accepted
        };
        var item2 = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            FinanceSessionId = session.Id,
            PayerId = session.UserId,
            ReceiverId = session.ReaderId,
            AmountDiamond = 20,
            Status = QuestionItemStatus.Accepted
        };
        var ignoredItem = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            FinanceSessionId = session.Id,
            PayerId = session.UserId,
            ReceiverId = session.ReaderId,
            AmountDiamond = 99,
            Status = QuestionItemStatus.Refunded
        };

        var summary = await service.ApplySessionReleaseAsync(
            session,
            new List<ChatQuestionItem> { item1, item2, ignoredItem },
            isAutoRelease: false,
            CancellationToken.None);

        Assert.NotNull(summary);
        Assert.Equal(session.Id, summary!.FinanceSessionId);
        Assert.Equal(30, summary.GrossAmountDiamond);
        Assert.Equal(3, summary.FeeAmountDiamond);
        Assert.Equal(27, summary.ReleasedAmountDiamond);
        Assert.Equal(2, summary.ReleasedItemCount);
        Assert.False(summary.IsAutoRelease);

        _walletRepositoryMock.Verify(
            x => x.ReleaseAsync(
                session.UserId,
                session.ReaderId,
                27,
                "chat_finance_session",
                session.Id.ToString(),
                It.IsAny<string>(),
                null,
                $"settle_session_release_{session.Id}",
                It.IsAny<CancellationToken>()),
            Times.Once);
        _walletRepositoryMock.Verify(
            x => x.ConsumeAsync(
                session.UserId,
                3,
                "platform_fee",
                session.Id.ToString(),
                It.IsAny<string>(),
                null,
                $"settle_session_fee_{session.Id}",
                It.IsAny<CancellationToken>()),
            Times.Once);

        _financeRepositoryMock.Verify(x => x.UpdateItemAsync(item1, It.IsAny<CancellationToken>()), Times.Once);
        _financeRepositoryMock.Verify(x => x.UpdateItemAsync(item2, It.IsAny<CancellationToken>()), Times.Once);
        _financeRepositoryMock.Verify(x => x.UpdateItemAsync(ignoredItem, It.IsAny<CancellationToken>()), Times.Never);
        _financeRepositoryMock.Verify(x => x.UpdateSessionAsync(session, It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(QuestionItemStatus.Released, item1.Status);
        Assert.Equal(QuestionItemStatus.Released, item2.Status);
        Assert.Equal(QuestionItemStatus.Refunded, ignoredItem.Status);
        Assert.Equal(0, session.TotalFrozen);
        Assert.Equal(ChatFinanceSessionStatus.Completed, session.Status);

        _domainEventPublisherMock.Verify(
            x => x.PublishAsync(
                It.Is<EscrowSessionReleasedDomainEvent>(e =>
                    e.FinanceSessionId == session.Id
                    && e.PayerId == session.UserId
                    && e.ReceiverId == session.ReaderId
                    && e.GrossAmountDiamond == 30
                    && e.FeeAmountDiamond == 3
                    && e.ReleasedAmountDiamond == 27
                    && e.ReleasedItemCount == 2
                    && e.IsAutoRelease == false),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ApplySessionReleaseAsync_ShouldReturnNull_WhenNoAcceptedItems()
    {
        _systemConfigSettingsMock.SetupGet(x => x.WithdrawalFeeRate).Returns(0.10m);
        _systemConfigSettingsMock.SetupGet(x => x.EscrowDisputeWindowHours).Returns(48);

        var service = new EscrowSettlementService(
            _financeRepositoryMock.Object,
            _walletRepositoryMock.Object,
            _domainEventPublisherMock.Object,
            _systemConfigSettingsMock.Object);

        var session = new ChatFinanceSession
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ReaderId = Guid.NewGuid(),
            TotalFrozen = 0,
            Status = ChatFinanceSessionStatus.Active
        };
        var items = new List<ChatQuestionItem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FinanceSessionId = session.Id,
                Status = QuestionItemStatus.Refunded,
                AmountDiamond = 10
            }
        };

        var summary = await service.ApplySessionReleaseAsync(
            session,
            items,
            isAutoRelease: true,
            CancellationToken.None);

        Assert.Null(summary);
        _walletRepositoryMock.Verify(
            x => x.ReleaseAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<long>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        _walletRepositoryMock.Verify(
            x => x.ConsumeAsync(
                It.IsAny<Guid>(),
                It.IsAny<long>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        _domainEventPublisherMock.Verify(
            x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
