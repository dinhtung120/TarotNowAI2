using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.ConfirmRelease;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

public class ConfirmReleaseCommandHandlerTests
{
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly ConfirmReleaseCommandHandler _handler;

    public ConfirmReleaseCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _handler = new ConfirmReleaseCommandHandler(_mockFinanceRepo.Object, _mockWalletRepo.Object);
    }

    [Fact]
    public async Task Handle_NotPayer_ThrowsBadRequest()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = Guid.NewGuid() }; // Different ID
        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ItemNotAccepted_ThrowsBadRequest()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = command.UserId, Status = QuestionItemStatus.Refunded };
        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NotReplied_ThrowsBadRequest()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = command.UserId, Status = QuestionItemStatus.Accepted, RepliedAt = null };
        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Reader chưa trả lời", ex.Message);
    }

    [Fact]
    public async Task Handle_AlreadyReleased_ThrowsBadRequest()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = command.UserId, Status = QuestionItemStatus.Accepted, RepliedAt = DateTime.UtcNow, ReleasedAt = DateTime.UtcNow };
        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidRequest_ReleasesAndConsumesFee()
    {
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem 
        { 
            Id = command.ItemId,
            PayerId = command.UserId, 
            ReceiverId = Guid.NewGuid(),
            AmountDiamond = 100, // Fee 10% = 10, reader=90
            Status = QuestionItemStatus.Accepted, 
            RepliedAt = DateTime.UtcNow,
            FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, TotalFrozen = 100 };
        
        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionByIdAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(QuestionItemStatus.Released, item.Status);
        Assert.NotNull(item.ReleasedAt);
        Assert.NotNull(item.DisputeWindowStart);
        Assert.NotNull(item.DisputeWindowEnd);

        _mockWalletRepo.Verify(x => x.ReleaseAsync(item.PayerId, item.ReceiverId, 90, "chat_question_item", item.Id.ToString(), It.IsAny<string>(), null, $"release_{item.Id}", default), Times.Once);
        _mockWalletRepo.Verify(x => x.ConsumeAsync(item.PayerId, 10, "platform_fee", item.Id.ToString(), It.IsAny<string>(), null, $"fee_{item.Id}", default), Times.Once);

        Assert.Equal(0, session.TotalFrozen);
        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(item, default), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, default), Times.Once);
    }

    /// <summary>
    /// TEST CASE: Item không tồn tại → NotFoundException.
    ///
    /// Khi nào xảy ra?
    /// → ItemId sai hoặc đã bị xóa khỏi hệ thống.
    ///   Quan trọng cho API security — không leak thông tin qua response.
    /// </summary>
    [Fact]
    public async Task Handle_ItemNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default))
            .ReturnsAsync((ChatQuestionItem)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: Fee rounding edge case — amount = 1 Diamond.
    ///
    /// Tại sao test này cần?
    /// → Fee 10% của 1 Diamond = 0.1 → Math.Ceiling = 1.
    ///   Reader nhận 0 Diamond! Đây là edge case đặc biệt.
    ///   Verify handler xử lý đúng: fee = 1, readerAmount = 0.
    ///   ReleaseAsync vẫn gọi với amount = 0, ConsumeAsync với fee = 1.
    /// </summary>
    [Fact]
    public async Task Handle_SmallAmount_FeeRoundingCorrect()
    {
        // Arrange — amount = 1 Diamond
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem
        {
            Id = command.ItemId,
            PayerId = command.UserId,
            ReceiverId = Guid.NewGuid(),
            AmountDiamond = 1, // Fee 10% = Ceiling(0.1) = 1, reader = 0
            Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow,
            FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, TotalFrozen = 1 };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionByIdAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);

        // Release với readerAmount = 0 (1 - 1 = 0)
        _mockWalletRepo.Verify(x => x.ReleaseAsync(
            item.PayerId, item.ReceiverId, 0,
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), $"release_{item.Id}", default), Times.Once);

        // Fee consume = 1
        _mockWalletRepo.Verify(x => x.ConsumeAsync(
            item.PayerId, 1,
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), $"fee_{item.Id}", default), Times.Once);
    }

    /// <summary>
    /// TEST CASE: Verify ConfirmedAt được set và AutoReleaseAt bị clear khi confirm.
    ///
    /// Tại sao cần test riêng?
    /// → AutoReleaseAt timer phải được clear khi user confirm trước.
    ///   Nếu không clear: auto-release job vẫn chạy → double release.
    ///   ConfirmedAt cần set để audit trail phân biệt user confirm vs auto-release.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRelease_ClearsAutoReleaseAndSetsConfirmedAt()
    {
        // Arrange
        var command = new ConfirmReleaseCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem
        {
            Id = command.ItemId,
            PayerId = command.UserId,
            ReceiverId = Guid.NewGuid(),
            AmountDiamond = 100,
            Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow,
            AutoReleaseAt = DateTime.UtcNow.AddHours(20), // Timer đang chạy
            FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, TotalFrozen = 100 };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionByIdAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert — AutoReleaseAt phải bị clear (cancel timer)
        Assert.Null(item.AutoReleaseAt);
        // Assert — ConfirmedAt phải được set (audit trail)
        Assert.NotNull(item.ConfirmedAt);
    }
}
