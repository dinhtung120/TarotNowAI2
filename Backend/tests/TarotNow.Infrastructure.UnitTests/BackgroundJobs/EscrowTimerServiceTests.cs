

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Services;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.BackgroundJobs;
using Xunit;

namespace TarotNow.Infrastructure.UnitTests.BackgroundJobs;

public class EscrowTimerServiceTests
{
    private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
    private readonly Mock<IServiceScope> _mockScope;
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly Mock<IConversationRepository> _mockConversationRepo;
    private readonly Mock<IChatMessageRepository> _mockMessageRepo;
    private readonly Mock<IDomainEventPublisher> _mockDomainEventPublisher;
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly Mock<ILogger<EscrowTimerService>> _mockLogger;
    private readonly IEscrowSettlementService _settlementService;
    private readonly EscrowTimerService _service;

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
        _mockLogger = new Mock<ILogger<EscrowTimerService>>();
        _settlementService = new EscrowSettlementService(
            _mockFinanceRepo.Object,
            _mockWalletRepo.Object,
            _mockDomainEventPublisher.Object);

        _mockScopeFactory.Setup(x => x.CreateScope()).Returns(_mockScope.Object);
        _mockScope.Setup(x => x.ServiceProvider).Returns(_mockServiceProvider.Object);

        
        _mockServiceProvider.Setup(x => x.GetService(typeof(IChatFinanceRepository))).Returns(_mockFinanceRepo.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IWalletRepository))).Returns(_mockWalletRepo.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IConversationRepository))).Returns(_mockConversationRepo.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IChatMessageRepository))).Returns(_mockMessageRepo.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IEscrowSettlementService))).Returns(_settlementService);
        _mockServiceProvider.Setup(x => x.GetService(typeof(ITransactionCoordinator))).Returns(_mockTransactionCoordinator.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IDomainEventPublisher))).Returns(_mockDomainEventPublisher.Object);

        _mockConversationRepo
            .Setup(x => x.GetConversationsAwaitingCompletionResolutionAsync(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ConversationDto>());
        _mockFinanceRepo
            .Setup(x => x.GetDisputedItemsForAutoResolveAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ChatQuestionItem>());
        _mockMessageRepo
            .Setup(x => x.GetExpiredPendingPaymentOffersAsync(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ChatMessageDto>());
        
        
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));

        _service = new EscrowTimerService(_mockScopeFactory.Object, _mockLogger.Object);
    }

        private async Task InvokeProcessTimersAsync()
    {
        var method = typeof(EscrowTimerService).GetMethod("ProcessTimers", BindingFlags.NonPublic | BindingFlags.Instance);
        var task = (Task)method!.Invoke(_service, new object[] { CancellationToken.None })!;
        await task;
    }

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

        await InvokeProcessTimersAsync();

        Assert.Equal(QuestionItemStatus.Refunded, expiredItem.Status);
        Assert.NotNull(expiredItem.RefundedAt);
        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(expiredItem, It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

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

        await InvokeProcessTimersAsync();

        Assert.Equal(QuestionItemStatus.Refunded, item.Status);
        Assert.NotNull(item.RefundedAt);
        Assert.Equal(0, session.TotalFrozen); 
        Assert.Equal("refunded", session.Status);

        _mockWalletRepo.Verify(x => x.RefundAsync(item.PayerId, item.AmountDiamond, "chat_question_item", item.Id.ToString(), It.IsAny<string>(), null, $"settle_refund_{item.Id}", It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(item, It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

        [Fact]
    public async Task ProcessAutoReleases_ReleasesConsumesFeeAndUpdatesSession()
    {
        var item = new ChatQuestionItem
        {
            Id = Guid.NewGuid(),
            PayerId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            AmountDiamond = 100,
            FinanceSessionId = Guid.NewGuid(),
            Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow.AddMinutes(-2),
            AutoReleaseAt = DateTime.UtcNow.AddMinutes(-1) 
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, TotalFrozen = 100 };

        _mockFinanceRepo.Setup(x => x.GetExpiredOffersAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoRefundAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem>());
        _mockFinanceRepo.Setup(x => x.GetItemsForAutoReleaseAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ChatQuestionItem> { item });
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(item.Id, It.IsAny<CancellationToken>())).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, It.IsAny<CancellationToken>())).ReturnsAsync(session);

        await InvokeProcessTimersAsync();

        Assert.Equal(QuestionItemStatus.Released, item.Status);
        Assert.NotNull(item.ReleasedAt);
        Assert.Equal(0, session.TotalFrozen); 

        
        _mockWalletRepo.Verify(x => x.ReleaseAsync(item.PayerId, item.ReceiverId, 90, "chat_question_item", item.Id.ToString(), It.IsAny<string>(), null, $"settle_release_{item.Id}", It.IsAny<CancellationToken>()), Times.Once);
        _mockWalletRepo.Verify(x => x.ConsumeAsync(item.PayerId, 10, "platform_fee", item.Id.ToString(), It.IsAny<string>(), null, $"settle_fee_{item.Id}", It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(item, It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, It.IsAny<CancellationToken>()), Times.Once);
    }
}
