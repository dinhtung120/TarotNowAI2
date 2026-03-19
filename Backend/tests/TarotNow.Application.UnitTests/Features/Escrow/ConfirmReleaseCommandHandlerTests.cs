/*
 * FILE: ConfirmReleaseCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler xác nhận giải phóng escrow (Confirm Release).
 *
 *   CÁC TEST CASE (7 scenarios):
 *   1. Handle_NotPayer_ThrowsBadRequest: không phải người trả → 400
 *   2. Handle_ItemNotAccepted_ThrowsBadRequest: item đã refund/released → 400
 *   3. Handle_NotReplied_ThrowsBadRequest: Reader chưa trả lời → 400
 *   4. Handle_AlreadyReleased_ThrowsBadRequest: đã release rồi → 400
 *   5. Handle_ValidRequest_ReleasesAndConsumesFee:
 *      → Happy path: release 90% cho Reader + consume 10% platform fee
 *   6. Handle_ItemNotFound_ThrowsNotFoundException: ItemId sai → 404
 *   7. Handle_SmallAmount_FeeRoundingCorrect: 1 Diamond → fee=1, reader=0 (Math.Ceiling)
 *   8. Handle_ValidRelease_ClearsAutoReleaseAndSetsConfirmedAt:
 *      → Clear AutoReleaseAt (cancel timer) + set ConfirmedAt (audit trail)
 *
 *   FEE FORMULA: fee = Math.Ceiling(amount * 0.10), readerAmount = amount - fee
 *   EDGE CASE: 1 Diamond → fee=1, reader=0 (toàn bộ là fee!)
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.ConfirmRelease;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

/// <summary>
/// Test confirm release: fee calculation, timer management, ACID settlement.
/// </summary>
public class ConfirmReleaseCommandHandlerTests
{
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly ConfirmReleaseCommandHandler _handler;

    public ConfirmReleaseCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns<Func<CancellationToken, Task>, CancellationToken>((action, ct) => action(ct));

        _handler = new ConfirmReleaseCommandHandler(
            _mockFinanceRepo.Object, _mockWalletRepo.Object,
            _mockTransactionCoordinator.Object);
    }

    /// <summary>Không phải payer → BadRequest.</summary>
    [Fact]
    public async Task Handle_NotPayer_ThrowsBadRequest()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = Guid.NewGuid() };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Item không ở Accepted status → BadRequest.</summary>
    [Fact]
    public async Task Handle_ItemNotAccepted_ThrowsBadRequest()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = command.UserId, Status = QuestionItemStatus.Refunded };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Reader chưa trả lời → BadRequest.</summary>
    [Fact]
    public async Task Handle_NotReplied_ThrowsBadRequest()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = command.UserId, Status = QuestionItemStatus.Accepted, RepliedAt = null };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Reader chưa trả lời", ex.Message);
    }

    /// <summary>Đã release rồi → BadRequest (idempotent guard).</summary>
    [Fact]
    public async Task Handle_AlreadyReleased_ThrowsBadRequest()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = command.UserId, Status = QuestionItemStatus.Accepted, RepliedAt = DateTime.UtcNow, ReleasedAt = DateTime.UtcNow };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Happy path: release 90 Diamond cho Reader + consume 10 Diamond platform fee.
    /// Verify: DisputeWindow set, TotalFrozen reset về 0.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_ReleasesAndConsumesFee()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem 
        { 
            Id = command.ItemId, PayerId = command.UserId, 
            ReceiverId = Guid.NewGuid(), AmountDiamond = 100,
            Status = QuestionItemStatus.Accepted, 
            RepliedAt = DateTime.UtcNow, FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, TotalFrozen = 100 };
        
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(QuestionItemStatus.Released, item.Status);
        Assert.NotNull(item.ReleasedAt);
        Assert.NotNull(item.DisputeWindowStart);
        Assert.NotNull(item.DisputeWindowEnd);

        // Release 90 cho Reader + Consume 10 platform fee
        _mockWalletRepo.Verify(x => x.ReleaseAsync(item.PayerId, item.ReceiverId, 90, "chat_question_item", item.Id.ToString(), It.IsAny<string>(), null, $"settle_release_{item.Id}", default), Times.Once);
        _mockWalletRepo.Verify(x => x.ConsumeAsync(item.PayerId, 10, "platform_fee", item.Id.ToString(), It.IsAny<string>(), null, $"settle_fee_{item.Id}", default), Times.Once);

        Assert.Equal(0, session.TotalFrozen);
    }

    /// <summary>ItemId sai → NotFoundException.</summary>
    [Fact]
    public async Task Handle_ItemNotFound_ThrowsNotFoundException()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync((ChatQuestionItem)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Edge case: 1 Diamond → fee=Ceiling(0.1)=1, reader=0.
    /// Reader nhận 0 Diamond! Verify handler xử lý đúng.
    /// </summary>
    [Fact]
    public async Task Handle_SmallAmount_FeeRoundingCorrect()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem
        {
            Id = command.ItemId, PayerId = command.UserId, ReceiverId = Guid.NewGuid(),
            AmountDiamond = 1, Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow, FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, TotalFrozen = 1 };

        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        _mockWalletRepo.Verify(x => x.ReleaseAsync(item.PayerId, item.ReceiverId, 0, // Reader = 0!
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), $"settle_release_{item.Id}", default), Times.Once);
        _mockWalletRepo.Verify(x => x.ConsumeAsync(item.PayerId, 1, // Fee = 1
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), $"settle_fee_{item.Id}", default), Times.Once);
    }

    /// <summary>
    /// Clear AutoReleaseAt (cancel timer) + set ConfirmedAt (audit trail).
    /// Nếu không clear: auto-release job chạy → double release.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRelease_ClearsAutoReleaseAndSetsConfirmedAt()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem
        {
            Id = command.ItemId, PayerId = command.UserId, ReceiverId = Guid.NewGuid(),
            AmountDiamond = 100, Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow,
            AutoReleaseAt = DateTime.UtcNow.AddHours(20), // Timer đang chạy
            FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, TotalFrozen = 100 };

        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Null(item.AutoReleaseAt); // Timer bị clear
        Assert.NotNull(item.ConfirmedAt); // Audit trail
    }
}
