using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Escrow.Commands.OpenDispute;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Escrow;

// Unit test cho handler mở tranh chấp item escrow.
public class OpenDisputeCommandHandlerRequestedDomainEventHandlerTests
{
    // Mock finance repo để điều khiển item/session và kiểm tra cập nhật trạng thái.
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    // Mock transaction coordinator để thực thi action transactional ngay trong test.
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly Mock<ISystemConfigSettings> _mockSystemConfigSettings;
    // Handler cần kiểm thử.
    private readonly OpenDisputeCommandHandlerRequestedDomainEventHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho OpenDisputeCommandHandlerRequestedDomainEventHandler.
    /// Luồng setup transaction coordinator inline giúp test deterministic.
    /// </summary>
    public OpenDisputeCommandHandlerRequestedDomainEventHandlerTests()
    {
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockSystemConfigSettings = new Mock<ISystemConfigSettings>();
        _mockSystemConfigSettings.SetupGet(x => x.EscrowDisputeMinReasonLength).Returns(10);
        _mockSystemConfigSettings.SetupGet(x => x.EscrowDisputeWindowHours).Returns(48);
        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken ct) => action(ct));
        _handler = new OpenDisputeCommandHandlerRequestedDomainEventHandler(
            _mockFinanceRepo.Object,
            _mockTransactionCoordinator.Object,
            _mockSystemConfigSettings.Object,
            Mock.Of<TarotNow.Application.Interfaces.DomainEvents.IEventHandlerIdempotencyService>());
    }

    /// <summary>
    /// Xác nhận user không phải payer/receiver của item sẽ bị từ chối.
    /// Luồng này bảo vệ quyền mở tranh chấp theo participant thực tế.
    /// </summary>
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

    /// <summary>
    /// Xác nhận item không ở trạng thái Accepted không thể mở tranh chấp.
    /// Luồng này bảo vệ state machine của question item.
    /// </summary>
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

    /// <summary>
    /// Xác nhận lý do tranh chấp quá ngắn bị từ chối.
    /// Luồng này đảm bảo dispute reason đủ thông tin cho vận hành xử lý.
    /// </summary>
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

    /// <summary>
    /// Xác nhận payer mở tranh chấp hợp lệ sẽ chuyển item sang Disputed và session sang disputed.
    /// Luồng này kiểm tra dispute window được thiết lập hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_FromPayer_UpdatesStatus()
    {
        const string disputeReason = "Valid dispute reason";
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid(), Reason = disputeReason };
        var item = new ChatQuestionItem
        {
            Id = command.ItemId, PayerId = command.UserId, Status = QuestionItemStatus.Accepted,
            AutoReleaseAt = DateTime.UtcNow.AddHours(1),
            FinanceSessionId = Guid.NewGuid()
        };
        var session = new ChatFinanceSession { Id = item.FinanceSessionId, Status = "active" };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync(item);
        _mockFinanceRepo.Setup(x => x.GetSessionForUpdateAsync(item.FinanceSessionId, default)).ReturnsAsync(session);

        // Thực thi handler và xác minh toàn bộ state transition kỳ vọng.
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        Assert.Equal(QuestionItemStatus.Disputed, item.Status);
        Assert.Equal("disputed", session.Status);
        Assert.NotNull(item.DisputeWindowStart);
        Assert.NotNull(item.DisputeWindowEnd);
        Assert.Equal(disputeReason, item.DisputeReason);
        Assert.True(item.DisputeWindowEnd > item.DisputeWindowStart);
    }

    /// <summary>
    /// Xác nhận receiver cũng có thể mở tranh chấp hợp lệ.
    /// Luồng này đảm bảo hai phía participant đều được phép dispute.
    /// </summary>
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

        // Thực thi handler và xác nhận item/session cùng chuyển đúng trạng thái.
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.True(result);
        Assert.Equal(QuestionItemStatus.Disputed, item.Status);
        Assert.Equal("disputed", session.Status);
        Assert.NotNull(item.DisputeWindowStart);
        Assert.NotNull(item.DisputeWindowEnd);
        Assert.Equal(command.Reason, item.DisputeReason);
        Assert.True(item.DisputeWindowEnd > item.DisputeWindowStart);
    }

    /// <summary>
    /// Xác nhận item không tồn tại trả NotFoundException.
    /// Luồng này chặn thao tác dispute trên dữ liệu không hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_ItemNotFound_ThrowsNotFoundException()
    {
        var command = new OpenDisputeCommand { ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() };
        _mockFinanceRepo.Setup(x => x.GetItemForUpdateAsync(command.ItemId, default)).ReturnsAsync((ChatQuestionItem)null!);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận item đã Disputed được xử lý idempotent.
    /// Luồng này không update/save thêm để tránh side-effect lặp.
    /// </summary>
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
