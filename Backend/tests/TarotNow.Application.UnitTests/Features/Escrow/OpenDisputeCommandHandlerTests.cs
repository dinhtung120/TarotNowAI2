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

        [Fact]
    public async Task Handle_NotParticipant_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var item = new ChatQuestionItem
        {
            PayerId = Guid.NewGuid(),
            ReceiverId = Guid.NewGuid(),
            Status = QuestionItemStatus.Accepted
        };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

        [Fact]
    public async Task Handle_NotAccepted_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand
        {
            ItemId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Reason = "Lý do tranh chấp hợp lệ"
        };
        var item = new ChatQuestionItem
        {
            PayerId = command.UserId,
            Status = QuestionItemStatus.Released
        };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

        [Fact]
    public async Task Handle_ShortReason_ThrowsBadRequest()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid(), Reason = "short" };
        var item = new ChatQuestionItem
        {
            PayerId = command.UserId,
            Status = QuestionItemStatus.Accepted
        };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

        [Fact]
    public async Task Handle_ValidRequest_FromPayer_UpdatesStatus()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid(), Reason = "Valid dispute reason" };
        var item = new ChatQuestionItem
        {
            Id = command.ItemId, PayerId = command.UserId, Status = QuestionItemStatus.Accepted,
            AutoReleaseAt = DateTime.UtcNow.AddHours(1),
            FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, Status = "active" };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        Assert.Equal(QuestionItemStatus.Disputed, item.Status);
        Assert.Equal("disputed", session.Status);
        Assert.NotNull(item.DisputeWindowStart);
        Assert.NotNull(item.DisputeWindowEnd);
        Assert.True(item.DisputeWindowEnd > item.DisputeWindowStart);
    }

        [Fact]
    public async Task Handle_ValidRequest_FromReceiver_UpdatesStatus()
    {
        var receiverId = Guid.NewGuid();
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = receiverId, Reason = "Valid dispute reason" };
        var item = new ChatQuestionItem
        {
            Id = command.ItemId,
            PayerId = Guid.NewGuid(),
            ReceiverId = receiverId,
            Status = QuestionItemStatus.Accepted,
            FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, Status = "active" };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        Assert.Equal(QuestionItemStatus.Disputed, item.Status);
        Assert.Equal("disputed", session.Status);
        Assert.NotNull(item.DisputeWindowStart);
        Assert.NotNull(item.DisputeWindowEnd);
        Assert.True(item.DisputeWindowEnd > item.DisputeWindowStart);
    }

        [Fact]
    public async Task Handle_ItemNotFound_ThrowsNotFoundException()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync((ChatQuestionItem)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

        [Fact]
    public async Task Handle_AlreadyDisputed_IsIdempotent()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid(), Reason = "Lý do tranh chấp hợp lệ" };
        var item = new ChatQuestionItem
        {
            PayerId = command.UserId,
            Status = QuestionItemStatus.Disputed,
            FinanceSessionId = Guid.NewGuid()
        };

        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        _mockFinanceRepo.Verify(x => x.UpdateItemAsync(It.IsAny<ChatQuestionItem>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockFinanceRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
