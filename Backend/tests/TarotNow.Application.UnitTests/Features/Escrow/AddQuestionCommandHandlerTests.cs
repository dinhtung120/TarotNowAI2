

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

// Unit test cho handler thêm câu hỏi trả phí trong phiên chat finance.
public class AddQuestionCommandExecutorTests
{
    // Mock finance repo để điều khiển session/item và kiểm tra persistence.
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    // Mock wallet repo để xác minh thao tác freeze diamond.
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    // Mock transaction coordinator để chạy action theo transactional flow.
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    // Mock publisher để xác nhận emit MoneyChangedDomainEvent.
    private readonly Mock<IDomainEventPublisher> _mockDomainEventPublisher;
    // Mock system config để đọc policy thời hạn escrow.
    private readonly Mock<ISystemConfigSettings> _mockSystemConfigSettings;
    // Handler cần kiểm thử.
    private readonly AddQuestionCommandExecutor _handler;

    /// <summary>
    /// Khởi tạo fixture cho AddQuestionCommandExecutor.
    /// Luồng cấu hình transaction coordinator chạy inline để test deterministic.
    /// </summary>
    public AddQuestionCommandExecutorTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockDomainEventPublisher = new Mock<IDomainEventPublisher>();
        _mockSystemConfigSettings = new Mock<ISystemConfigSettings>();
        _mockSystemConfigSettings.SetupGet(x => x.EscrowReaderResponseDueHours).Returns(24);
        _mockSystemConfigSettings.SetupGet(x => x.EscrowAutoRefundHours).Returns(24);

        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));

        _handler = new AddQuestionCommandExecutor(
            _mockFinanceRepo.Object, _mockWalletRepo.Object,
            _mockTransactionCoordinator.Object, _mockDomainEventPublisher.Object,
            _mockSystemConfigSettings.Object);
    }

    /// <summary>
    /// Xác nhận idempotency key đã tồn tại sẽ trả item hiện có và không freeze ví lần nữa.
    /// Luồng này bảo vệ tính idempotent khi client retry request.
    /// </summary>
    [Fact]
    public async Task Handle_ExistingIdempotencyKey_ReturnsExistingId_NoFreeze()
    {
        var command = new AddQuestionCommand { IdempotencyKey = "addq_key_123" };
        var existingItem = new ChatQuestionItem { Id = Guid.NewGuid() };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("addq_key_123", default))
            .ReturnsAsync(existingItem);

        var result = await _handler.Handle(command, CancellationToken.None);

        // Đảm bảo không gọi freeze lần hai khi gặp request trùng idempotency key.
        Assert.Equal(existingItem.Id, result);
        _mockWalletRepo.Verify(x => x.FreezeAsync(
            It.IsAny<Guid>(), It.IsAny<long>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
    }

    /// <summary>
    /// Xác nhận session không tồn tại trả NotFoundException.
    /// Luồng này ngăn tạo item tài chính mồ côi.
    /// </summary>
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

    /// <summary>
    /// Xác nhận user không phải chủ session bị từ chối.
    /// Luồng này bảo vệ quyền thao tác tài chính theo chủ phiên.
    /// </summary>
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

    /// <summary>
    /// Xác nhận session đã kết thúc không cho thêm câu hỏi.
    /// Luồng này bảo vệ business rule đóng phiên tài chính.
    /// </summary>
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

    /// <summary>
    /// Xác nhận request hợp lệ sẽ freeze diamond, tạo item và cập nhật tổng frozen của session.
    /// Luồng này kiểm tra đầy đủ side-effect tài chính trong transaction.
    /// </summary>
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

        // Thực thi handler và kiểm tra các side-effect freeze + persistence.
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

        // Tổng frozen phải tăng đúng số diamond vừa thêm.
        Assert.Equal(150, session.TotalFrozen);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, default), Times.Once);
        _mockFinanceRepo.Verify(x => x.SaveChangesAsync(default), Times.Once);
        _mockDomainEventPublisher.Verify(
            x => x.PublishAsync(It.IsAny<MoneyChangedDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
