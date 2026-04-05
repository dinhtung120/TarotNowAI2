using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Call.Commands.RespondCall;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Call;

/// <summary>
/// Unit Tests kiểm tra chức năng RespondCallCommandHandler.
/// </summary>
public class RespondCallCommandHandlerTests
{
    private readonly Mock<ICallSessionRepository> _mockCallRepo;
    private readonly Mock<IConversationRepository> _mockConvRepo;
    private readonly RespondCallCommandHandler _handler;

    public RespondCallCommandHandlerTests()
    {
        _mockCallRepo = new Mock<ICallSessionRepository>();
        _mockConvRepo = new Mock<IConversationRepository>();
        _handler = new RespondCallCommandHandler(_mockCallRepo.Object, _mockConvRepo.Object);
    }

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

        _mockCallRepo.SetupSequence(r => r.GetByIdAsync(callId, default))
            .ReturnsAsync(callSession)
            .ReturnsAsync(new CallSessionDto
            {
                Status = CallSessionStatus.Accepted,
                StartedAt = DateTime.UtcNow
            });
        
        _mockConvRepo.Setup(r => r.GetByIdAsync("conv-1", default)).ReturnsAsync(conv);

        var command = new RespondCallCommand { CallSessionId = callId, ResponderId = responderId, Accept = true };
        var result = await _handler.Handle(command, CancellationToken.None);

        _mockCallRepo.Verify(x => x.UpdateStatusAsync(callId, CallSessionStatus.Accepted, It.IsNotNull<DateTime>(), null, null, default), Times.Once);
        Assert.Equal(CallSessionStatus.Accepted, result.Status);
    }

    [Fact]
    public async Task Handle_NotRequested_ThrowsBadRequest()
    {
        var callId = "call-1";
        var callSession = new CallSessionDto { Id = callId, Status = CallSessionStatus.Ended };

        _mockCallRepo.Setup(r => r.GetByIdAsync(callId, default)).ReturnsAsync(callSession);

        var command = new RespondCallCommand { CallSessionId = callId, ResponderId = Guid.NewGuid(), Accept = true };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

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
