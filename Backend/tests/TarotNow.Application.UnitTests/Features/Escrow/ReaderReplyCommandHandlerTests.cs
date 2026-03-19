/*
 * FILE: ReaderReplyCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler reader trả lời câu hỏi escrow.
 *
 *   CÁC TEST CASE (5 scenarios):
 *   1. ItemNotFound → NotFoundException
 *   2. NotReceiver → BadRequest (chỉ reader assigned mới reply được)
 *   3. ItemNotAccepted → BadRequest (chỉ Accepted items mới reply)
 *   4. AlreadyReplied → BadRequest (không cho reply 2 lần → chống gian lận thời gian)
 *   5. ValidReply → set RepliedAt + AutoReleaseAt=+24h + clear AutoRefundAt
 *
 *   STATE MACHINE: Accepted → (reply) → chờ user confirm/dispute hoặc auto-release
 *   TIMER: AutoRefundAt phải clear (reader đã reply, không cần auto-refund)
 *          AutoReleaseAt = replied_at + 24h
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.ReaderReply;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

/// <summary>
/// Test reader reply: timer management (AutoReleaseAt/AutoRefundAt), ownership check.
/// </summary>
public class ReaderReplyCommandHandlerTests
{
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly ReaderReplyCommandHandler _handler;

    public ReaderReplyCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));
        _handler = new ReaderReplyCommandHandler(_mockFinanceRepo.Object, _mockTransactionCoordinator.Object);
    }

    /// <summary>ItemId sai → NotFoundException.</summary>
    [Fact]
    public async Task Handle_ItemNotFound_ThrowsNotFoundException()
    {
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = Guid.NewGuid() };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync((ChatQuestionItem)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Không phải reader assigned → BadRequest.</summary>
    [Fact]
    public async Task Handle_NotReceiver_ThrowsBadRequestException()
    {
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = Guid.NewGuid() };
        var item = new ChatQuestionItem { ReceiverId = Guid.NewGuid() };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không phải reader", ex.Message);
    }

    /// <summary>Item không Accepted → BadRequest.</summary>
    [Fact]
    public async Task Handle_ItemNotAccepted_ThrowsBadRequestException()
    {
        var readerId = Guid.NewGuid();
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = readerId };
        var item = new ChatQuestionItem { ReceiverId = readerId, Status = QuestionItemStatus.Released };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Đã reply rồi → BadRequest (không cho reset AutoReleaseAt).</summary>
    [Fact]
    public async Task Handle_AlreadyReplied_ThrowsBadRequestException()
    {
        var readerId = Guid.NewGuid();
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = readerId };
        var item = new ChatQuestionItem { ReceiverId = readerId, Status = QuestionItemStatus.Accepted, RepliedAt = DateTime.UtcNow.AddHours(-1) };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("đã được trả lời", ex.Message);
    }

    /// <summary>
    /// Happy path: RepliedAt set, AutoReleaseAt=+24h, AutoRefundAt cleared.
    /// </summary>
    [Fact]
    public async Task Handle_ValidReply_SetsTimersCorrectly()
    {
        var readerId = Guid.NewGuid();
        var command = new ReaderReplyCommand { ItemId = Guid.NewGuid(), ReaderId = readerId };
        var item = new ChatQuestionItem
        {
            ReceiverId = readerId, Status = QuestionItemStatus.Accepted,
            RepliedAt = null, AutoRefundAt = DateTime.UtcNow.AddHours(20)
        };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);

        var beforeHandle = DateTime.UtcNow;
        var result = await _handler.Handle(command, CancellationToken.None);
        var afterHandle = DateTime.UtcNow;

        Assert.True(result);
        Assert.NotNull(item.RepliedAt);
        Assert.InRange(item.RepliedAt.Value, beforeHandle, afterHandle.AddSeconds(1));
        Assert.NotNull(item.AutoReleaseAt);
        var expectedAutoRelease = item.RepliedAt.Value.AddHours(24);
        Assert.InRange(item.AutoReleaseAt.Value, expectedAutoRelease.AddMinutes(-1), expectedAutoRelease.AddMinutes(1));
        Assert.Null(item.AutoRefundAt); // Phải clear!

        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(item, It.IsAny<CancellationToken>()), Times.Once);
        _mockFinanceRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
