

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

public class AddQuestionCommandHandlerTests
{
    
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly AddQuestionCommandHandler _handler;

    public AddQuestionCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));

        _handler = new AddQuestionCommandHandler(
            _mockFinanceRepo.Object, _mockWalletRepo.Object,
            _mockTransactionCoordinator.Object);
    }

        [Fact]
    public async Task Handle_ExistingIdempotencyKey_ReturnsExistingId_NoFreeze()
    {
        var command = new AddQuestionCommand { IdempotencyKey = "addq_key_123" };
        var existingItem = new ChatQuestionItem { Id = Guid.NewGuid() };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("addq_key_123", default))
            .ReturnsAsync(existingItem);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(existingItem.Id, result);
        _mockWalletRepo.Verify(x => x.FreezeAsync(
            It.IsAny<Guid>(), It.IsAny<long>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
    }

        [Fact]
    public async Task Handle_SessionNotFound_ThrowsNotFoundException()
    {
        var command = new AddQuestionCommand { ConversationRef = "non_existent_conv", IdempotencyKey = "new_key" };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("new_key", default))
            .ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("non_existent_conv", default))
            .ReturnsAsync((ChatFinanceSession)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

        [Fact]
    public async Task Handle_NotSessionOwner_ThrowsBadRequestException()
    {
        var command = new AddQuestionCommand { UserId = Guid.NewGuid(), ConversationRef = "conv_ref", IdempotencyKey = "new_key" };
        var session = new ChatFinanceSession { UserId = Guid.NewGuid(), Status = "active" }; 

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("new_key", default)).ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("conv_ref", default)).ReturnsAsync(session);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không phải chủ phiên", ex.Message);
    }

        [Fact]
    public async Task Handle_SessionEnded_ThrowsBadRequestException()
    {
        var userId = Guid.NewGuid();
        var command = new AddQuestionCommand { UserId = userId, ConversationRef = "conv_ref", IdempotencyKey = "new_key" };
        var session = new ChatFinanceSession { UserId = userId, Status = "completed" };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("new_key", default)).ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("conv_ref", default)).ReturnsAsync(session);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không thể thêm câu hỏi", ex.Message);
    }

        [Fact]
    public async Task Handle_ValidRequest_FreezesDiamond_CreatesItem_UpdatesSession()
    {
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var command = new AddQuestionCommand
        {
            UserId = userId, ConversationRef = "conv_ref",
            AmountDiamond = 50, IdempotencyKey = "addq_key_456"
        };
        var session = new ChatFinanceSession
        {
            Id = sessionId, UserId = userId, ReaderId = readerId,
            Status = "active", TotalFrozen = 100 
        };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("addq_key_456", default)).ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("conv_ref", default)).ReturnsAsync(session);

        var createdItemId = Guid.NewGuid();
        _mockFinanceRepo.Setup(x => x.AddItemAsync(It.IsAny<ChatQuestionItem>(), default))
            .Callback<ChatQuestionItem, CancellationToken>((item, _) => item.Id = createdItemId)
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(createdItemId, result);
        _mockWalletRepo.Verify(x => x.FreezeAsync(userId, 50, "chat_question_item", "addq_key_456",
            It.IsAny<string>(), It.IsAny<string>(), "freeze_addq_key_456", default), Times.Once);
        _mockFinanceRepo.Verify(x => x.AddItemAsync(
            It.Is<ChatQuestionItem>(i =>
                i.FinanceSessionId == sessionId && i.PayerId == userId &&
                i.ReceiverId == readerId && i.Type == QuestionItemType.AddQuestion &&
                i.AmountDiamond == 50 && i.Status == QuestionItemStatus.Accepted &&
                i.IdempotencyKey == "addq_key_456"), default), Times.Once);

        
        Assert.Equal(150, session.TotalFrozen);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, default), Times.Once);
        _mockFinanceRepo.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}
