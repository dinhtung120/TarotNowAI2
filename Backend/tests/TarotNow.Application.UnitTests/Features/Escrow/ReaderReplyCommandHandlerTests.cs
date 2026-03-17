using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.ReaderReply;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

/// <summary>
/// Unit tests cho ReaderReplyCommandHandler — xử lý reader trả lời câu hỏi escrow.
///
/// Tại sao handler này quan trọng?
/// → Khi reader reply:
///   1. Set RepliedAt → đánh dấu đã trả lời
///   2. Set AutoReleaseAt = +24h → nếu user không dispute trong 24h, auto-release
///   3. Clear AutoRefundAt → vì reader đã trả lời, không cần auto-refund nữa
///
/// Đây là bước chuyển trạng thái quan trọng trong escrow state machine:
///   accepted → (reader reply) → chờ user confirm/dispute hoặc auto-release
///
/// Nếu bugs ở handler này:
/// - AutoReleaseAt không set → timer job không chạy → Diamond bị kẹt mãi mãi
/// - AutoRefundAt không clear → auto-refund chạy SAU khi reader đã reply → sai logic
/// </summary>
public class ReaderReplyCommandHandlerTests
{
    /* Mock repository — chỉ cần finance repo vì reply không liên quan wallet */
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly ReaderReplyCommandHandler _handler;

    public ReaderReplyCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _handler = new ReaderReplyCommandHandler(_mockFinanceRepo.Object);
    }

    /// <summary>
    /// TEST CASE: Item không tồn tại → NotFoundException.
    ///
    /// Tại sao cần test?
    /// → Phòng trường hợp ItemId sai hoặc đã bị delete.
    ///   Handler phải throw NotFoundException rõ ràng thay vì NullReferenceException.
    /// </summary>
    [Fact]
    public async Task Handle_ItemNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = Guid.NewGuid() };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default))
            .ReturnsAsync((ChatQuestionItem)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: Người gọi không phải reader nhận → BadRequestException.
    ///
    /// Tại sao cần check?
    /// → Bảo mật: chỉ reader được assigned (ReceiverId) mới có quyền reply.
    ///   Nếu không check, user hoặc reader khác có thể reply giả → gian lận escrow.
    /// </summary>
    [Fact]
    public async Task Handle_NotReceiver_ThrowsBadRequestException()
    {
        // Arrange — ReceiverId khác với ReaderId trong command
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = Guid.NewGuid() };
        var item = new ChatQuestionItem { ReceiverId = Guid.NewGuid() }; // Người khác

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default))
            .ReturnsAsync(item);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không phải reader", ex.Message);
    }

    /// <summary>
    /// TEST CASE: Item không ở trạng thái Accepted → BadRequestException.
    ///
    /// Tại sao cần check status?
    /// → State machine: chỉ accepted items (đã freeze Diamond) mới reply được.
    ///   Nếu item đã released/refunded/disputed → reply không có ý nghĩa.
    /// </summary>
    [Fact]
    public async Task Handle_ItemNotAccepted_ThrowsBadRequestException()
    {
        // Arrange — item đã released → không thể reply
        var readerId = Guid.NewGuid();
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = readerId };
        var item = new ChatQuestionItem
        {
            ReceiverId = readerId,
            Status = QuestionItemStatus.Released // Trạng thái sai
        };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default))
            .ReturnsAsync(item);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: Item đã được reply rồi (RepliedAt != null) → BadRequestException.
    ///
    /// Tại sao cần idempotency-like check?
    /// → Tránh reader reply nhiều lần → mỗi lần reset AutoReleaseAt,
    ///   kéo dài thời gian user phải chờ → gian lận thời gian.
    /// </summary>
    [Fact]
    public async Task Handle_AlreadyReplied_ThrowsBadRequestException()
    {
        // Arrange — item đã replied
        var readerId = Guid.NewGuid();
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = readerId };
        var item = new ChatQuestionItem
        {
            ReceiverId = readerId,
            Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow.AddHours(-1) // Đã reply 1 giờ trước
        };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default))
            .ReturnsAsync(item);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("đã được trả lời", ex.Message);
    }

    /// <summary>
    /// TEST CASE: Happy path — reader reply hợp lệ.
    ///
    /// Verify 3 điều quan trọng:
    /// 1. RepliedAt = now → đánh dấu thời điểm reader reply
    /// 2. AutoReleaseAt = now + 24h → timer cho auto-release nếu user không dispute
    /// 3. AutoRefundAt = null → xóa timer auto-refund vì reader đã reply
    ///
    /// Tại sao phải clear AutoRefundAt?
    /// → Khi accept, AutoRefundAt = accepted_at + 24h (nếu reader không reply).
    ///   Nhưng reader vừa reply → timer auto-refund không còn ý nghĩa.
    ///   Nếu không clear → background job auto-refund sẽ chạy SAU khi đã reply
    ///   → refund trong khi reader đã hoàn thành → mất tiền reader.
    /// </summary>
    [Fact]
    public async Task Handle_ValidReply_SetsTimersCorrectly()
    {
        // Arrange
        var readerId = Guid.NewGuid();
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = readerId };
        var item = new ChatQuestionItem
        {
            ReceiverId = readerId,
            Status = QuestionItemStatus.Accepted,
            RepliedAt = null,
            AutoRefundAt = DateTime.UtcNow.AddHours(20) // Timer auto-refund đang chạy
        };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default))
            .ReturnsAsync(item);

        // Act
        var beforeHandle = DateTime.UtcNow;
        var result = await _handler.Handle(command, CancellationToken.None);
        var afterHandle = DateTime.UtcNow;

        // Assert — trả về true
        Assert.True(result);

        // Assert — RepliedAt được set (trong khoảng thời gian test chạy)
        Assert.NotNull(item.RepliedAt);
        Assert.InRange(item.RepliedAt.Value, beforeHandle, afterHandle.AddSeconds(1));

        // Assert — AutoReleaseAt = replied_at + 24h (tolerance 1 phút)
        Assert.NotNull(item.AutoReleaseAt);
        var expectedAutoRelease = item.RepliedAt.Value.AddHours(24);
        Assert.InRange(item.AutoReleaseAt.Value,
            expectedAutoRelease.AddMinutes(-1),
            expectedAutoRelease.AddMinutes(1));

        // Assert — AutoRefundAt phải bị clear (rất quan trọng!)
        Assert.Null(item.AutoRefundAt);

        // Assert — UpdateItemAsync + SaveChangesAsync được gọi
        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(item, default), Times.Once);
        _mockFinanceRepo.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}
