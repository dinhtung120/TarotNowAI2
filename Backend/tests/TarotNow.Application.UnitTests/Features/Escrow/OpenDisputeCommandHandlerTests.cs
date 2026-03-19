/*
 * FILE: OpenDisputeCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler mở tranh chấp (Open Dispute) trong Escrow.
 *
 *   CÁC TEST CASE (7 scenarios):
 *   1. NotPayer → 400
 *   2. NotReleased → 400
 *   3. OutsideWindow → 400
 *   4. ShortReason → 400
 *   5. ValidRequest → Disputed
 *   6. ItemNotFound → 404
 *   7. ReaderNotReplied → 400
 *
 *   STATE MACHINE: Accepted (replied) → Disputed (trong dispute window)
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

/// <summary>
/// Test open dispute: window check, state transition, reason validation.
/// </summary>
public class OpenDisputeCommandHandlerTests
{
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly OpenDisputeCommandHandler _handler;

    public OpenDisputeCommandHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));
        _handler = new OpenDisputeCommandHandler(_mockFinanceRepo.Object, _mockTransactionCoordinator.Object);
    }

    /// <summary>Không phải payer → BadRequest.</summary>
    [Fact]
    public async Task Handle_NotPayer_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = Guid.NewGuid() };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Item Released → BadRequest.</summary>
    [Fact]
    public async Task Handle_NotReleased_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem { PayerId = command.UserId, Status = QuestionItemStatus.Released };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Quá dispute window → BadRequest.</summary>
    [Fact]
    public async Task Handle_OutsideWindow_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem 
        { 
            PayerId = command.UserId, Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow.AddMinutes(-10),
            AutoReleaseAt = DateTime.UtcNow.AddHours(-1)
        };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("quá thời hạn mở tranh chấp", ex.Message);
    }

    /// <summary>Lý do ngắn → BadRequest.</summary>
    [Fact]
    public async Task Handle_ShortReason_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid(), Reason = "short" };
        var item = new ChatQuestionItem 
        { 
            PayerId = command.UserId, Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow.AddMinutes(-10),
            AutoReleaseAt = DateTime.UtcNow.AddHours(1)
        };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Happy path: item→Disputed, session→disputed.</summary>
    [Fact]
    public async Task Handle_ValidRequest_UpdatesStatus()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid(), Reason = "Valid dispute reason" };
        var item = new ChatQuestionItem 
        { 
            Id = command.ItemId, PayerId = command.UserId, Status = QuestionItemStatus.Accepted,
            RepliedAt = DateTime.UtcNow.AddMinutes(-5), AutoReleaseAt = DateTime.UtcNow.AddHours(1),
            FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, Status = "active" };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        Assert.Equal(QuestionItemStatus.Disputed, item.Status);
        Assert.Equal("disputed", session.Status);
    }

    /// <summary>ItemId sai → NotFoundException.</summary>
    [Fact]
    public async Task Handle_ItemNotFound_ThrowsNotFoundException()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync((ChatQuestionItem)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Reader chưa reply → BadRequest.</summary>
    [Fact]
    public async Task Handle_ReaderNotReplied_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid(), Reason = "Valid dispute reason" };
        var item = new ChatQuestionItem { PayerId = command.UserId, Status = QuestionItemStatus.Accepted, RepliedAt = null };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Reader chưa trả lời", ex.Message);
    }
}
