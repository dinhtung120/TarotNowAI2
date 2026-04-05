using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Call.Commands.EndCall;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Call;

/// <summary>
/// Unit Tests kiểm tra chức năng EndCallCommandHandler.
/// </summary>
public class EndCallCommandHandlerTests
{
    private readonly Mock<ICallSessionRepository> _mockCallRepo;
    private readonly Mock<IConversationRepository> _mockConvRepo;
    private readonly EndCallCommandHandler _handler;

    public EndCallCommandHandlerTests()
    {
        _mockCallRepo = new Mock<ICallSessionRepository>();
        _mockConvRepo = new Mock<IConversationRepository>();
        _handler = new EndCallCommandHandler(_mockCallRepo.Object, _mockConvRepo.Object);
    }

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

        var command = new EndCallCommand { CallSessionId = callId, UserId = userId, Reason = "normal" };
        await _handler.Handle(command, CancellationToken.None);

        _mockCallRepo.Verify(x => x.UpdateStatusAsync(callId, CallSessionStatus.Ended, null, It.IsNotNull<DateTime>(), "normal", default), Times.Once);
    }

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
        
        // Người dùng callee phải Respond Reject chứ không EndCall
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
