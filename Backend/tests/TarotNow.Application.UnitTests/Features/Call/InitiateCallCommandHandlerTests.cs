using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Call.Commands.InitiateCall;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Call;

/// <summary>
/// Unit Tests kiểm tra chức năng InitiateCallCommandHandler.
/// </summary>
public class InitiateCallCommandHandlerTests
{
    private readonly Mock<ICallSessionRepository> _mockCallRepo;
    private readonly Mock<IConversationRepository> _mockConvRepo;
    private readonly InitiateCallCommandHandler _handler;

    public InitiateCallCommandHandlerTests()
    {
        _mockCallRepo = new Mock<ICallSessionRepository>();
        _mockConvRepo = new Mock<IConversationRepository>();
        _handler = new InitiateCallCommandHandler(_mockCallRepo.Object, _mockConvRepo.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesRequestedSession()
    {
        // Thông tin test
        var conversationId = "conv-123";
        var initiatorId = Guid.NewGuid();
        var conv = new ConversationDto { Id = conversationId, Status = "ongoing", UserId = initiatorId.ToString() };

        _mockConvRepo.Setup(r => r.GetByIdAsync(conversationId, default)).ReturnsAsync(conv);
        _mockCallRepo.Setup(r => r.GetActiveByConversationAsync(conversationId, default)).ReturnsAsync((CallSessionDto)null!);

        var command = new InitiateCallCommand { ConversationId = conversationId, InitiatorId = initiatorId, Type = "video" };
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(CallSessionStatus.Requested, result.Status);
        Assert.Equal(CallType.Video, result.Type);
        Assert.Equal(initiatorId.ToString(), result.InitiatorId);

        _mockCallRepo.Verify(x => x.AddAsync(It.IsAny<CallSessionDto>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_ConversationNotOngoing_ThrowsBadRequest()
    {
        var conversationId = "conv-123";
        var initiatorId = Guid.NewGuid();
        // Trạng thái đã pending không cho phép gọi
        var conv = new ConversationDto { Id = conversationId, Status = "pending", UserId = initiatorId.ToString() };

        _mockConvRepo.Setup(r => r.GetByIdAsync(conversationId, default)).ReturnsAsync(conv);

        var command = new InitiateCallCommand { ConversationId = conversationId, InitiatorId = initiatorId, Type = "audio" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NonParticipant_ThrowsForbidden()
    {
        var conversationId = "conv-123";
        var initiatorId = Guid.NewGuid();
        var conv = new ConversationDto { Id = conversationId, Status = "ongoing", UserId = Guid.NewGuid().ToString(), ReaderId = Guid.NewGuid().ToString() };

        _mockConvRepo.Setup(r => r.GetByIdAsync(conversationId, default)).ReturnsAsync(conv);

        var command = new InitiateCallCommand { ConversationId = conversationId, InitiatorId = initiatorId, Type = "audio" };
        await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ExistingActiveCall_ThrowsBadRequest()
    {
        var conversationId = "conv-123";
        var initiatorId = Guid.NewGuid();
        var conv = new ConversationDto { Id = conversationId, Status = "ongoing", UserId = initiatorId.ToString() };
        var activeCall = new CallSessionDto { Id = "call-99", Status = CallSessionStatus.Requested };

        _mockConvRepo.Setup(r => r.GetByIdAsync(conversationId, default)).ReturnsAsync(conv);
        _mockCallRepo.Setup(r => r.GetActiveByConversationAsync(conversationId, default)).ReturnsAsync(activeCall);

        var command = new InitiateCallCommand { ConversationId = conversationId, InitiatorId = initiatorId, Type = "audio" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
