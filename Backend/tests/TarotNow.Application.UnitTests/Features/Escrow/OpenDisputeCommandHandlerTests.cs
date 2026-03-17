using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

public class OpenDisputeCommandHandlerTests
{
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly OpenDisputeCommandHandler _handler;

    public OpenDisputeCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _handler = new OpenDisputeCommandHandler(_mockFinanceRepo.Object);
    }

    [Fact]
    public async Task Handle_NotPayer_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = Guid.NewGuid() }; // Different ID
        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NotReleased_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = command.UserId, Status = QuestionItemStatus.Accepted };
        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_OutsideWindow_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem 
        { 
            PayerId = command.UserId, 
            Status = QuestionItemStatus.Released,
            DisputeWindowEnd = DateTime.UtcNow.AddHours(-1) // Passed
        };
        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Đã hết thời hạn mở tranh chấp", ex.Message);
    }

    [Fact]
    public async Task Handle_ShortReason_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid(), Reason = "short" };
        var item = new ChatQuestionItem 
        { 
            PayerId = command.UserId, 
            Status = QuestionItemStatus.Released,
            DisputeWindowEnd = DateTime.UtcNow.AddHours(1)
        };
        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesStatus()
    {
        var command = new OpenDisputeCommand 
        { 
            ItemId = Guid.NewGuid(), 
            UserId = Guid.NewGuid(), 
            Reason = "Valid dispute reason"
        };
        var item = new ChatQuestionItem 
        { 
            Id = command.ItemId,
            PayerId = command.UserId, 
            Status = QuestionItemStatus.Released,
            DisputeWindowEnd = DateTime.UtcNow.AddHours(1),
            FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, Status = "active" };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionByIdAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(QuestionItemStatus.Disputed, item.Status);
        Assert.Equal("disputed", session.Status);

        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(item, default), Times.Once);
        _mockFinanceRepo.Verify(x => x.UpdateSessionAsync(session, default), Times.Once);
    }

    /// <summary>
    /// TEST CASE: Item không tồn tại → NotFoundException.
    ///
    /// Tại sao cần test?
    /// → Phòng trường hợp ItemId sai.
    ///   Đặc biệt quan trọng vì dispute là hành động nhạy cảm (liên quan tài chính).
    /// </summary>
    [Fact]
    public async Task Handle_ItemNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default))
            .ReturnsAsync((ChatQuestionItem)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: DisputeWindowEnd = null → BadRequestException.
    ///
    /// Khi nào xảy ra?
    /// → Item vừa được release nhưng chưa set DisputeWindowEnd (lỗi logic).
    /// → Hoặc auto-release mà backend chưa set window.
    ///   Handler phải reject vì không xác định được deadline.
    /// </summary>
    [Fact]
    public async Task Handle_NullDisputeWindow_ThrowsBadRequest()
    {
        // Arrange — DisputeWindowEnd = null
        var command = new OpenDisputeCommand
        {
            ItemId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Reason = "Valid dispute reason"
        };
        var item = new ChatQuestionItem
        {
            PayerId = command.UserId,
            Status = QuestionItemStatus.Released,
            DisputeWindowEnd = null // Chưa set window
        };

        _mockFinanceRepo.Setup(x => x.GetItemByIdAsync(command.ItemId, default))
            .ReturnsAsync(item);

        // Act & Assert — reject vì now > null → cần check handler logic
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("hết thời hạn", ex.Message);
    }
}
