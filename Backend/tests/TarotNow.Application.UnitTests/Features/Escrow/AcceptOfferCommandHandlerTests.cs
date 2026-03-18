using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.AcceptOffer;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

public class AcceptOfferCommandHandlerTests
{
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly AcceptOfferCommandHandler _handler;

    public AcceptOfferCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));

        _handler = new AcceptOfferCommandHandler(
            _mockFinanceRepo.Object,
            _mockWalletRepo.Object,
            _mockTransactionCoordinator.Object);
    }

    [Fact]
    public async Task Handle_ExistingIdempotencyKey_ReturnsExistingItemId()
    {
        var command = new AcceptOfferCommand { IdempotencyKey = "key123" };
        var existingItem = new ChatQuestionItem { Id = Guid.NewGuid() };
        
        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("key123", default)).ReturnsAsync(existingItem);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(existingItem.Id, result);
        _mockWalletRepo.Verify(x => x.FreezeAsync(It.IsAny<Guid>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_NewOffer_CreatesSessionAndItem_FreezesDiamond()
    {
        var command = new AcceptOfferCommand 
        { 
            UserId = Guid.NewGuid(), 
            ReaderId = Guid.NewGuid(), 
            ConversationRef = "conv_ref", 
            AmountDiamond = 100, 
            IdempotencyKey = "key123" 
        };
        
        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("key123", default)).ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("conv_ref", default)).ReturnsAsync((ChatFinanceSession)null!);

        var expectedId = Guid.NewGuid();
        _mockFinanceRepo.Setup(x => x.AddItemAsync(It.IsAny<ChatQuestionItem>(), default))
            .Callback<ChatQuestionItem, CancellationToken>((i, c) => i.Id = expectedId)
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(expectedId, result);
        
        // Assert session created
        _mockFinanceRepo.Verify(x => x.AddSessionAsync(It.Is<ChatFinanceSession>(s => 
            s.ConversationRef == "conv_ref" && s.TotalFrozen == 100), default), Times.Once);
            
        // Assert freeze called
        _mockWalletRepo.Verify(x => x.FreezeAsync(command.UserId, 100, "chat_question_item", "key123", It.IsAny<string>(), null, "freeze_key123", default), Times.Once);
        
        // Assert item created
        _mockFinanceRepo.Verify(x => x.AddItemAsync(It.Is<ChatQuestionItem>(i => 
            i.PayerId == command.UserId && i.AmountDiamond == 100 && i.Status == QuestionItemStatus.Accepted), default), Times.Once);
    }
}
