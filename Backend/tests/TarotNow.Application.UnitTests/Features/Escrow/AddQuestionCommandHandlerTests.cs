using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.AddQuestion;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

/// <summary>
/// Unit tests cho AddQuestionCommandHandler — xử lý thêm câu hỏi vào phiên escrow.
///
/// Tại sao cần test riêng handler này?
/// → Add-question là tính năng cộng dồn escrow: mỗi câu hỏi thêm sẽ freeze thêm Diamond,
///   tăng total_frozen của session. Nếu không test kỹ, có thể gây double-freeze
///   hoặc quên cộng dồn total_frozen → sai số dư.
///
/// Chiến lược test:
/// 1. Idempotency — kiểm tra không freeze trùng lặp
/// 2. Session validation — session phải tồn tại, thuộc về user, còn active
/// 3. Happy path — freeze + tạo item + cộng dồn total_frozen
/// </summary>
public class AddQuestionCommandHandlerTests
{
    /* Mock các repository — tách biệt handler logic khỏi database thực */
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly AddQuestionCommandHandler _handler;

    public AddQuestionCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _handler = new AddQuestionCommandHandler(_mockFinanceRepo.Object, _mockWalletRepo.Object);
    }

    /// <summary>
    /// TEST CASE: Idempotency — nếu IdempotencyKey đã tồn tại,
    /// trả về ID cũ mà không gọi Freeze lần nữa.
    ///
    /// Tại sao quan trọng?
    /// → Trong distributed system, request có thể bị retry (network hiccup).
    ///   Nếu không check idempotency, user bị freeze Diamond 2 lần cho 1 câu hỏi.
    ///   Đây là yêu cầu bắt buộc từ BR-12 (Finance invariants).
    /// </summary>
    [Fact]
    public async Task Handle_ExistingIdempotencyKey_ReturnsExistingId_NoFreeze()
    {
        // Arrange — giả lập đã có item với key này (retry scenario)
        var command = new AddQuestionCommand { IdempotencyKey = "addq_key_123" };
        var existingItem = new ChatQuestionItem { Id = Guid.NewGuid() };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("addq_key_123", default))
            .ReturnsAsync(existingItem);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert — trả về ID cũ, KHÔNG gọi Freeze
        Assert.Equal(existingItem.Id, result);
        _mockWalletRepo.Verify(x => x.FreezeAsync(
            It.IsAny<Guid>(), It.IsAny<long>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
    }

    /// <summary>
    /// TEST CASE: Session không tồn tại → NotFoundException.
    ///
    /// Tại sao cần validate?
    /// → Add-question chỉ hoạt động trên session đã có (user đã accept offer trước đó).
    ///   Nếu conversationRef không tồn tại = request bất hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_SessionNotFound_ThrowsNotFoundException()
    {
        // Arrange — không có item trùng key, nhưng session không tồn tại
        var command = new AddQuestionCommand
        {
            ConversationRef = "non_existent_conv",
            IdempotencyKey = "new_key"
        };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("new_key", default))
            .ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("non_existent_conv", default))
            .ReturnsAsync((ChatFinanceSession)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: User không phải chủ phiên → BadRequestException.
    ///
    /// Tại sao cần check ownership?
    /// → Bảo mật: chỉ payer (user đã tạo session) mới được thêm câu hỏi.
    ///   Reader hoặc user khác không được phép freeze Diamond của người khác.
    /// </summary>
    [Fact]
    public async Task Handle_NotSessionOwner_ThrowsBadRequestException()
    {
        // Arrange — session thuộc về userId khác
        var command = new AddQuestionCommand
        {
            UserId = Guid.NewGuid(),
            ConversationRef = "conv_ref",
            IdempotencyKey = "new_key"
        };
        var session = new ChatFinanceSession
        {
            UserId = Guid.NewGuid(), // Khác với command.UserId
            Status = "active"
        };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("new_key", default))
            .ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("conv_ref", default))
            .ReturnsAsync(session);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không phải chủ phiên", ex.Message);
    }

    /// <summary>
    /// TEST CASE: Session đã kết thúc (completed/refunded/cancelled) → BadRequestException.
    ///
    /// Tại sao cần check status?
    /// → State machine: sau khi session kết thúc, không được phép thêm câu hỏi nữa.
    ///   Nếu cho phép → freeze Diamond vô nghĩa, dẫn đến tiền bị kẹt.
    /// </summary>
    [Fact]
    public async Task Handle_SessionEnded_ThrowsBadRequestException()
    {
        // Arrange — session đã completed
        var userId = Guid.NewGuid();
        var command = new AddQuestionCommand
        {
            UserId = userId,
            ConversationRef = "conv_ref",
            IdempotencyKey = "new_key"
        };
        var session = new ChatFinanceSession
        {
            UserId = userId,
            Status = "completed" // Session đã kết thúc
        };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("new_key", default))
            .ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("conv_ref", default))
            .ReturnsAsync(session);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không thể thêm câu hỏi", ex.Message);
    }

    /// <summary>
    /// TEST CASE: Happy path — request hợp lệ → freeze Diamond, tạo item type=add_question,
    /// cộng dồn total_frozen trong session.
    ///
    /// Đây là test quan trọng nhất — verify toàn bộ luồng business:
    /// 1. FreezeAsync được gọi với đúng amount
    /// 2. Item mới được tạo với type=AddQuestion, status=Accepted
    /// 3. Session.TotalFrozen tăng thêm đúng amount (cộng dồn)
    /// 4. SaveChangesAsync được gọi (đảm bảo ACID)
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_FreezesDiamond_CreatesItem_UpdatesSession()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var command = new AddQuestionCommand
        {
            UserId = userId,
            ConversationRef = "conv_ref",
            AmountDiamond = 50,
            IdempotencyKey = "addq_key_456"
        };
        var session = new ChatFinanceSession
        {
            Id = sessionId,
            UserId = userId,
            ReaderId = readerId,
            Status = "active",
            TotalFrozen = 100 // Đã có 100 từ main question
        };

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("addq_key_456", default))
            .ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("conv_ref", default))
            .ReturnsAsync(session);

        var createdItemId = Guid.NewGuid();
        _mockFinanceRepo.Setup(x => x.AddItemAsync(It.IsAny<ChatQuestionItem>(), default))
            .Callback<ChatQuestionItem, CancellationToken>((item, _) => item.Id = createdItemId)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert — trả về ID mới
        Assert.Equal(createdItemId, result);

        // Assert — FreezeAsync được gọi đúng params
        _mockWalletRepo.Verify(x => x.FreezeAsync(
            userId, 50,
            "chat_question_item", "addq_key_456",
            It.IsAny<string>(),
            It.IsAny<string>(),
            "freeze_addq_key_456",
            default), Times.Once);

        // Assert — Item tạo đúng type = AddQuestion
        _mockFinanceRepo.Verify(x => x.AddItemAsync(
            It.Is<ChatQuestionItem>(i =>
                i.FinanceSessionId == sessionId &&
                i.PayerId == userId &&
                i.ReceiverId == readerId &&
                i.Type == QuestionItemType.AddQuestion &&
                i.AmountDiamond == 50 &&
                i.Status == QuestionItemStatus.Accepted &&
                i.IdempotencyKey == "addq_key_456"),
            default), Times.Once);

        // Assert — Total frozen cộng dồn: 100 + 50 = 150
        Assert.Equal(150, session.TotalFrozen);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, default), Times.Once);
        _mockFinanceRepo.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}
