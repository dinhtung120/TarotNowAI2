using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Call.Commands.EndCall;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Call;

// Unit test cho handler kết thúc cuộc gọi.
public class EndCallCommandHandlerTests
{
    // Mock call repo để điều khiển trạng thái call session.
    private readonly Mock<ICallSessionRepository> _mockCallRepo;
    // Mock conversation repo để kiểm tra quyền người dùng theo hội thoại.
    private readonly Mock<IConversationRepository> _mockConvRepo;
    // Handler cần kiểm thử.
    private readonly EndCallCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho EndCallCommandHandler.
    /// Luồng dùng mock repositories để kiểm thử logic chuyển trạng thái cuộc gọi.
    /// </summary>
    public EndCallCommandHandlerTests()
    {
        _mockCallRepo = new Mock<ICallSessionRepository>();
        _mockConvRepo = new Mock<IConversationRepository>();
        _handler = new EndCallCommandHandler(_mockCallRepo.Object, _mockConvRepo.Object);
    }

    /// <summary>
    /// Xác nhận cuộc gọi đã accepted sẽ được cập nhật trạng thái Ended.
    /// Luồng kiểm tra handler gọi UpdateStatusAsync với endedAt khác null.
    /// </summary>
    [Fact]
    public async Task Handle_EndAcceptedCall_StatusEnded()
    {
        var callId = "call-1";
        var userId = Guid.NewGuid();

        var callSession = new CallSessionDto
        {
            Id = callId,
            ConversationId = "conv-1",
            Status = CallSessionStatus.Accepted,
            StartedAt = DateTime.UtcNow.AddMinutes(-5)
        };
        var conv = new ConversationDto { Id = "conv-1", UserId = userId.ToString() };

        _mockCallRepo.Setup(r => r.GetByIdAsync(callId, default)).ReturnsAsync(callSession);
        _mockConvRepo.Setup(r => r.GetByIdAsync("conv-1", default)).ReturnsAsync(conv);
        _mockCallRepo.Setup(x => x.UpdateStatusAsync(
                callId,
                CallSessionStatus.Ended,
                null,
                It.IsAny<DateTime?>(),
                "normal",
                CallSessionStatus.Accepted,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Gọi handler kết thúc call ở nhánh accepted.
        var command = new EndCallCommand { CallSessionId = callId, UserId = userId, Reason = "normal" };
        await _handler.Handle(command, CancellationToken.None);

        // Xác nhận repository nhận đúng tham số chuyển trạng thái sang Ended.
        _mockCallRepo.Verify(x => x.UpdateStatusAsync(
            callId,
            CallSessionStatus.Ended,
            null,
            It.IsNotNull<DateTime>(),
            "normal",
            CallSessionStatus.Accepted,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Xác nhận callee không được hủy call ở trạng thái Requested do không phải initiator.
    /// Luồng này kiểm tra rule phân quyền thao tác end/cancel call.
    /// </summary>
    [Fact]
    public async Task Handle_CancelRequestedCallByCallee_ThrowsBadRequest()
    {
        var callId = "call-1";
        var calleeId = Guid.NewGuid();
        var initiatorIdStr = Guid.NewGuid().ToString();

        var callSession = new CallSessionDto
        {
            Id = callId,
            ConversationId = "conv-1",
            Status = CallSessionStatus.Requested,
            InitiatorId = initiatorIdStr
        };
        var conv = new ConversationDto { Id = "conv-1", UserId = calleeId.ToString(), ReaderId = initiatorIdStr };

        _mockCallRepo.Setup(r => r.GetByIdAsync(callId, default)).ReturnsAsync(callSession);
        _mockConvRepo.Setup(r => r.GetByIdAsync("conv-1", default)).ReturnsAsync(conv);

        var command = new EndCallCommand { CallSessionId = callId, UserId = calleeId, Reason = "normal" };
        // Kỳ vọng ném BadRequest khi người gọi không phải initiator cố gắng cancel.
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
