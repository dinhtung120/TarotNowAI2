using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Call.Commands.RespondCall;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Call;

// Unit test cho handler phản hồi cuộc gọi (accept/reject).
public class RespondCallCommandHandlerTests
{
    // Mock call repo để điều khiển trạng thái call session trước/sau phản hồi.
    private readonly Mock<ICallSessionRepository> _mockCallRepo;
    // Mock conversation repo để xác minh quyền responder.
    private readonly Mock<IConversationRepository> _mockConvRepo;
    // Handler cần kiểm thử.
    private readonly RespondCallCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho RespondCallCommandHandler.
    /// Luồng dùng mock repositories để cô lập logic phản hồi cuộc gọi.
    /// </summary>
    public RespondCallCommandHandlerTests()
    {
        _mockCallRepo = new Mock<ICallSessionRepository>();
        _mockConvRepo = new Mock<IConversationRepository>();
        _handler = new RespondCallCommandHandler(_mockCallRepo.Object, _mockConvRepo.Object);
    }

    /// <summary>
    /// Xác nhận nhánh accept chuyển trạng thái sang Accepted và set StartedAt.
    /// Luồng kiểm tra UpdateStatusAsync được gọi với startedAt khác null.
    /// </summary>
    [Fact]
    public async Task Handle_AcceptCall_StatusAccepted_StartedAtSet()
    {
        var callId = "call-1";
        var responderId = Guid.NewGuid();
        var initiatorId = Guid.NewGuid();

        var callSession = new CallSessionDto
        {
            Id = callId,
            ConversationId = "conv-1",
            Status = CallSessionStatus.Requested,
            InitiatorId = initiatorId.ToString()
        };
        var conv = new ConversationDto
        {
            Id = "conv-1",
            UserId = responderId.ToString(),
            ReaderId = initiatorId.ToString()
        };

        // Setup sequence để giả lập call session sau update được đọc lại ở trạng thái Accepted.
        _mockCallRepo.SetupSequence(r => r.GetByIdAsync(callId, default))
            .ReturnsAsync(callSession)
            .ReturnsAsync(new CallSessionDto
            {
                Status = CallSessionStatus.Accepted,
                StartedAt = DateTime.UtcNow
            });
        
        _mockConvRepo.Setup(r => r.GetByIdAsync("conv-1", default)).ReturnsAsync(conv);
        _mockCallRepo.Setup(x => x.UpdateStatusAsync(
                callId,
                CallSessionStatus.Accepted,
                It.IsAny<DateTime?>(),
                null,
                null,
                CallSessionStatus.Requested,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Gọi handler accept call và xác minh call repo được gọi đúng tham số.
        var command = new RespondCallCommand { CallSessionId = callId, ResponderId = responderId, Accept = true };
        var result = await _handler.Handle(command, CancellationToken.None);

        _mockCallRepo.Verify(x => x.UpdateStatusAsync(
            callId,
            CallSessionStatus.Accepted,
            It.IsNotNull<DateTime>(),
            null,
            null,
            CallSessionStatus.Requested,
            It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(CallSessionStatus.Accepted, result.Status);
    }

    /// <summary>
    /// Xác nhận call không ở Requested thì không cho phản hồi.
    /// Luồng này bảo vệ state machine của call session.
    /// </summary>
    [Fact]
    public async Task Handle_NotRequested_ThrowsBadRequest()
    {
        var callId = "call-1";
        var callSession = new CallSessionDto { Id = callId, Status = CallSessionStatus.Ended };

        _mockCallRepo.Setup(r => r.GetByIdAsync(callId, default)).ReturnsAsync(callSession);

        var command = new RespondCallCommand { CallSessionId = callId, ResponderId = Guid.NewGuid(), Accept = true };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận initiator không được tự phản hồi cuộc gọi của chính mình.
    /// Luồng này kiểm tra rule phân tách vai trò caller và responder.
    /// </summary>
    [Fact]
    public async Task Handle_InitiatorCantRespond_ThrowsBadRequest()
    {
        var callId = "call-1";
        var initiatorId = Guid.NewGuid();
        var callSession = new CallSessionDto { Id = callId, Status = CallSessionStatus.Requested, ConversationId = "conv-1", InitiatorId = initiatorId.ToString() };
        var conv = new ConversationDto { Id = "conv-1", UserId = initiatorId.ToString() };

        _mockCallRepo.Setup(r => r.GetByIdAsync(callId, default)).ReturnsAsync(callSession);
        _mockConvRepo.Setup(r => r.GetByIdAsync("conv-1", default)).ReturnsAsync(conv);

        var command = new RespondCallCommand { CallSessionId = callId, ResponderId = initiatorId, Accept = true };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
