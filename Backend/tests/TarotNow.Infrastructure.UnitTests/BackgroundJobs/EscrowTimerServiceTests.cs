

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Services;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using TarotNow.Infrastructure.BackgroundJobs;
using Xunit;

namespace TarotNow.Infrastructure.UnitTests.BackgroundJobs;

// Unit test cho background job EscrowTimerService.
public class EscrowTimerServiceTests
{
    // Mock scope factory/provider để dựng dependency graph của background service.
    private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
    private readonly Mock<IServiceScope> _mockScope;
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    // Mock các repository/service phụ thuộc trong luồng timer settlement.
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly Mock<IConversationRepository> _mockConversationRepo;
    private readonly Mock<IChatMessageRepository> _mockMessageRepo;
    private readonly Mock<IDomainEventPublisher> _mockDomainEventPublisher;
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly Mock<ISystemConfigSettings> _mockSystemConfigSettings;
    private readonly Mock<ILogger<EscrowTimerService>> _mockLogger;
    // Settlement service thật để test end-to-end logic settle trong process timer.
    private readonly IEscrowSettlementService _settlementService;
    // Service cần kiểm thử.
    private readonly EscrowTimerService _service;

    /// <summary>
    /// Khởi tạo fixture cho EscrowTimerService.
    /// Luồng này giả lập DI scope và setup toàn bộ dependency mặc định cho các test timer.
    /// </summary>
    public EscrowTimerServiceTests()
    {
        _mockScopeFactory = new Mock<IServiceScopeFactory>();
        _mockScope = new Mock<IServiceScope>();
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockConversationRepo = new Mock<IConversationRepository>();
        _mockMessageRepo = new Mock<IChatMessageRepository>();
        _mockDomainEventPublisher = new Mock<IDomainEventPublisher>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockSystemConfigSettings = new Mock<ISystemConfigSettings>();
        _mockSystemConfigSettings.SetupGet(x => x.WithdrawalFeeRate).Returns(0.10m);
        _mockSystemConfigSettings.SetupGet(x => x.EscrowDisputeWindowHours).Returns(48);
        _mockLogger = new Mock<ILogger<EscrowTimerService>>();
        _settlementService = new EscrowSettlementService(
            _mockFinanceRepo.Object,
            _mockWalletRepo.Object,
            _mockDomainEventPublisher.Object,
            _mockSystemConfigSettings.Object);

        _mockScopeFactory.Setup(x => x.CreateScope()).Returns(_mockScope.Object);
        _mockScope.Setup(x => x.ServiceProvider).Returns(_mockServiceProvider.Object);

        // Đăng ký dependency vào service provider giả để background service resolve được.
        _mockServiceProvider.Setup(x => x.GetService(typeof(IChatFinanceRepository))).Returns(_mockFinanceRepo.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IWalletRepository))).Returns(_mockWalletRepo.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IConversationRepository))).Returns(_mockConversationRepo.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IChatMessageRepository))).Returns(_mockMessageRepo.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IEscrowSettlementService))).Returns(_settlementService);
        _mockServiceProvider.Setup(x => x.GetService(typeof(ITransactionCoordinator))).Returns(_mockTransactionCoordinator.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IDomainEventPublisher))).Returns(_mockDomainEventPublisher.Object);

        // Thiết lập dữ liệu rỗng mặc định để từng test chỉ override phần cần kiểm tra.
        _mockConversationRepo
            .Setup(x => x.GetConversationsAwaitingCompletionResolutionAsync(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ConversationDto>());
        _mockFinanceRepo
            .Setup(x => x.GetDisputedItemsForAutoResolveAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ChatQuestionItem>());
        _mockMessageRepo
            .Setup(x => x.GetExpiredPendingPaymentOffersAsync(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ChatMessageDto>());

        // ExecuteAsync chạy inline để assert side-effect ngay trong test thread.
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));

        _service = new EscrowTimerService(
            _mockScopeFactory.Object,
            _mockLogger.Object,
            _mockSystemConfigSettings.Object);
    }

    /// <summary>
    /// Gọi phương thức ProcessTimers nội bộ qua reflection.
    /// Luồng helper này cho phép test trực tiếp nhánh xử lý timer mà không cần chạy hosted service loop.
    /// </summary>
    private async Task InvokeProcessTimersAsync()
    {
        var method = typeof(EscrowTimerService).GetMethod("ProcessTimers", BindingFlags.NonPublic | BindingFlags.Instance);
        var task = (Task)method!.Invoke(_service, new object[] { CancellationToken.None })!;
        await task;
    }

    /// <summary>
    /// Xác nhận expired offer được chuyển về Refunded và persist.
    /// Luồng này kiểm tra nhánh hủy offer quá hạn.
    /// </summary>
    [Fact]
    public async Task ProcessExpiredOffers_CancelsOffer()
    {
        var expiredItem = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            Status = QuestionItemStatus.Pending,
            OfferExpiresAt = DateTime.UtcNow.AddMinutes(-1)
        };
        _mockFinanceRepo.Setup(x => x.GetExpiredOffersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem> { expiredItem });
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(expiredItem.Id, It.IsAny<CancellationToken>())).ReturnsAsync(expiredItem);
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoRefundAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoReleaseAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());

        // Chạy timer processing và assert trạng thái item đã được refund.
        await InvokeProcessTimersAsync();

        Assert.Equal(QuestionItemStatus.Refunded, expiredItem.Status);
        Assert.NotNull(expiredItem.RefundedAt);
        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(expiredItem, It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Xác nhận auto-refund cập nhật item, session và hoàn ví đúng số tiền.
    /// Luồng này kiểm tra settlement nhánh refund cho item quá hạn phản hồi.
    /// </summary>
    [Fact]
    public async Task ProcessAutoRefunds_RefundsAndUpdatesSession()
    {
        var item = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            PayerId = Guid.NewGuid(),
            AmountDiamond = 100, 
            FinanceSessionId = Guid.NewGuid(),
            Status = QuestionItemStatus.Accepted,
            AutoRefundAt = DateTime.UtcNow.AddMinutes(-1)
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, TotalFrozen = 100 };

        _mockFinanceRepo.Setup(x => x.GetExpiredOffersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoRefundAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem> { item });
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(item.Id, It.IsAny<CancellationToken>())).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, It.IsAny<CancellationToken>())).ReturnsAsync(session);
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoReleaseAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());

        // Chạy timer processing và kiểm tra side-effect refund đầy đủ.
        await InvokeProcessTimersAsync();

        Assert.Equal(QuestionItemStatus.Refunded, item.Status);
        Assert.NotNull(item.RefundedAt);
        Assert.Equal(0, session.TotalFrozen);
        Assert.Equal("refunded", session.Status);

        _mockWalletRepo.Verify(x => x.RefundAsync(item.PayerId, item.AmountDiamond, "chat_question_item", item.Id.ToString(), It.IsAny<string>(), null, $"settle_refund_{item.Id}", It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(item, It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    /// <summary>
    /// Xác nhận auto-release session-level sẽ release một lần cho receiver, consume phí nền tảng và cập nhật session.
    /// Luồng này kiểm tra settlement gộp khi toàn bộ accepted item trong session đã tới hạn.
    /// </summary>
    [Fact]
    public async Task ProcessAutoReleases_ReleasesConsumesFeeAndUpdatesSession()
    {
        var payerId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var item = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            PayerId = payerId,
            ReceiverId = receiverId,
            AmountDiamond = 100,
            FinanceSessionId = sessionId,
            Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow.AddMinutes(-2),
            AutoReleaseAt = DateTime.UtcNow.AddMinutes(-1)
        };
        var session = new ChatFinanceSession
        {
            Id = sessionId,
            ConversationRef = "conv-auto-release-1",
            UserId = payerId,
            ReaderId = receiverId,
            TotalFrozen = 100,
            Status = ChatFinanceSessionStatus.Active
        };

        _mockFinanceRepo.Setup(x => x.GetExpiredOffersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoRefundAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoReleaseAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem> { item });
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(sessionId, It.IsAny<CancellationToken>())).ReturnsAsync(session);
        _mockFinanceRepo.Setup(x => x.GetItemsBySessionIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ChatQuestionItem> { item });

        // Chạy timer processing và kiểm tra side-effect release + fee consume.
        await InvokeProcessTimersAsync();

        Assert.Equal(QuestionItemStatus.Released, item.Status);
        Assert.NotNull(item.ReleasedAt);
        Assert.Equal(0, session.TotalFrozen);

        // Release 90% cho receiver, 10% là phí nền tảng theo policy hiện tại (tính trên tổng session).
        _mockWalletRepo.Verify(x => x.ReleaseAsync(
            payerId,
            receiverId,
            90,
            "chat_finance_session",
            sessionId.ToString(),
            It.IsAny<string>(),
            null,
            $"settle_session_release_{sessionId}",
            It.IsAny<CancellationToken>()), Times.Once);
        _mockWalletRepo.Verify(x => x.ConsumeAsync(
            payerId,
            10,
            "platform_fee",
            sessionId.ToString(),
            It.IsAny<string>(),
            null,
            $"settle_session_fee_{sessionId}",
            It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(item, It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Xác nhận auto-release sẽ không settle partial khi session còn accepted item chưa đủ điều kiện.
    /// </summary>
    [Fact]
    public async Task ProcessAutoReleases_DoesNotSettle_WhenAnyAcceptedItemIsNotYetDue()
    {
        var payerId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var dueItem = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            PayerId = payerId,
            ReceiverId = receiverId,
            AmountDiamond = 10,
            FinanceSessionId = sessionId,
            Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow.AddMinutes(-3),
            AutoReleaseAt = DateTime.UtcNow.AddMinutes(-1)
        };
        var notDueItem = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            PayerId = payerId,
            ReceiverId = receiverId,
            AmountDiamond = 20,
            FinanceSessionId = sessionId,
            Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow.AddMinutes(-1),
            AutoReleaseAt = DateTime.UtcNow.AddHours(1)
        };
        var session = new ChatFinanceSession
        {
            Id = sessionId,
            ConversationRef = "conv-auto-release-2",
            UserId = payerId,
            ReaderId = receiverId,
            TotalFrozen = 30,
            Status = ChatFinanceSessionStatus.Active
        };

        _mockFinanceRepo.Setup(x => x.GetExpiredOffersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoRefundAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoReleaseAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem> { dueItem });
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(sessionId, It.IsAny<CancellationToken>())).ReturnsAsync(session);
        _mockFinanceRepo.Setup(x => x.GetItemsBySessionIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ChatQuestionItem> { dueItem, notDueItem });

        await InvokeProcessTimersAsync();

        _mockWalletRepo.Verify(x => x.ReleaseAsync(
            It.IsAny<Guid>(),
            It.IsAny<Guid>(),
            It.IsAny<long>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), Times.Never);
        _mockWalletRepo.Verify(x => x.ConsumeAsync(
            It.IsAny<Guid>(),
            It.IsAny<long>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<CancellationToken>()), Times.Never);

        Assert.Equal(QuestionItemStatus.Accepted, dueItem.Status);
        Assert.Equal(QuestionItemStatus.Accepted, notDueItem.Status);
        Assert.Equal(30, session.TotalFrozen);
    }

    [Fact]
    public async Task ProcessAutoRefunds_ShouldPublishConversationSyncInsideTransaction()
    {
        var item = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            ConversationRef = "conv-sync-1",
            PayerId = Guid.NewGuid(),
            AmountDiamond = 80,
            FinanceSessionId = Guid.NewGuid(),
            Status = QuestionItemStatus.Accepted,
            AutoRefundAt = DateTime.UtcNow.AddMinutes(-1)
        };
        var session = new ChatFinanceSession
        {
            Id = item.FinanceSessionId,
            TotalFrozen = 80,
            Status = ChatFinanceSessionStatus.Pending
        };
        var inTransaction = false;

        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns(async (Func<CancellationToken, Task> action, CancellationToken ct) =>
            {
                inTransaction = true;
                try
                {
                    await action(ct);
                }
                finally
                {
                    inTransaction = false;
                }
            });

        _mockDomainEventPublisher
            .Setup(x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns<IDomainEvent, CancellationToken>((domainEvent, _) =>
            {
                if (domainEvent is EscrowConversationSyncRequestedDomainEvent && !inTransaction)
                {
                    throw new InvalidOperationException("Escrow conversation sync event published outside transaction.");
                }

                return Task.CompletedTask;
            });

        _mockFinanceRepo.Setup(x => x.GetExpiredOffersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoRefundAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem> { item });
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(item.Id, It.IsAny<CancellationToken>())).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, It.IsAny<CancellationToken>())).ReturnsAsync(session);
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoReleaseAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());

        await InvokeProcessTimersAsync();

        _mockDomainEventPublisher.Verify(
            x => x.PublishAsync(
                It.Is<EscrowConversationSyncRequestedDomainEvent>(e =>
                    e.ConversationId == "conv-sync-1"
                    && e.SyncReason == "auto_refund"
                    && e.TargetStatus == ConversationStatus.Expired),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
