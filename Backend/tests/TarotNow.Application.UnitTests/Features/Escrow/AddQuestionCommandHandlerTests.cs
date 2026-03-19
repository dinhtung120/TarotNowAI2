/*
 * FILE: AddQuestionCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler thêm câu hỏi vào phiên Escrow.
 *
 *   CÁC TEST CASE (6 scenarios):
 *   1. Handle_ExistingIdempotencyKey_ReturnsExistingId_NoFreeze: chống double-freeze khi retry
 *   2. Handle_SessionNotFound_ThrowsNotFoundException: session không tồn tại → 404
 *   3. Handle_NotSessionOwner_ThrowsBadRequestException: không phải chủ phiên → 400
 *   4. Handle_SessionEnded_ThrowsBadRequestException: session đã kết thúc → 400
 *   5. Handle_ValidRequest_FreezesDiamond_CreatesItem_UpdatesSession:
 *      → Happy path: freeze thêm + tạo item AddQuestion + cộng dồn TotalFrozen
 *
 *   ESCROW CỘNG DỒN:
 *   → Mỗi add-question tăng TotalFrozen: 100 + 50 = 150
 *   → Type = AddQuestion (phân biệt với main question)
 */

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
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly AddQuestionCommandHandler _handler;

    public AddQuestionCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        // Mock transaction coordinator: thực thi action trực tiếp
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));

        _handler = new AddQuestionCommandHandler(
            _mockFinanceRepo.Object, _mockWalletRepo.Object,
            _mockTransactionCoordinator.Object);
    }

    /// <summary>
    /// IdempotencyKey đã tồn tại → trả ID cũ, KHÔNG gọi Freeze.
    /// Bắt buộc từ BR-12: chống double-freeze khi network retry.
    /// </summary>
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

    /// <summary>Session không tồn tại → NotFoundException.</summary>
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

    /// <summary>Không phải chủ phiên → BadRequest (bảo mật: chỉ payer mới add được).</summary>
    [Fact]
    public async Task Handle_NotSessionOwner_ThrowsBadRequestException()
    {
        var command = new AddQuestionCommand { UserId = Guid.NewGuid(), ConversationRef = "conv_ref", IdempotencyKey = "new_key" };
        var session = new ChatFinanceSession { UserId = Guid.NewGuid(), Status = "active" }; // Khác UserId

        _mockFinanceRepo.Setup(x => x.GetItemByIdempotencyKeyAsync("new_key", default)).ReturnsAsync((ChatQuestionItem)null!);
        _mockFinanceRepo.Setup(x => x.GetSessionByConversationRefAsync("conv_ref", default)).ReturnsAsync(session);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không phải chủ phiên", ex.Message);
    }

    /// <summary>Session đã kết thúc → BadRequest (state machine: không thêm câu hỏi).</summary>
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
    /// Happy path: freeze Diamond + tạo item AddQuestion + cộng dồn TotalFrozen.
    /// TotalFrozen: 100 (existing) + 50 (new) = 150.
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
            Status = "active", TotalFrozen = 100 // Đã có 100 từ main question
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

        // TotalFrozen cộng dồn: 100 + 50 = 150
        Assert.Equal(150, session.TotalFrozen);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, default), Times.Once);
        _mockFinanceRepo.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}
