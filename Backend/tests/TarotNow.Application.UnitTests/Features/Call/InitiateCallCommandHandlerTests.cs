using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Call.Commands.InitiateCall;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Call;

// Unit test cho handler khởi tạo cuộc gọi.
public class InitiateCallCommandHandlerTests
{
    // Mock call repo để kiểm soát active call và thao tác add call session.
    private readonly Mock<ICallSessionRepository> _mockCallRepo;
    // Mock conversation repo để kiểm tra trạng thái hội thoại và quyền participant.
    private readonly Mock<IConversationRepository> _mockConvRepo;
    // Handler cần kiểm thử.
    private readonly InitiateCallCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho InitiateCallCommandHandler.
    /// Luồng dùng mock repositories để kiểm thử độc lập logic nghiệp vụ.
    /// </summary>
    public InitiateCallCommandHandlerTests()
    {
        _mockCallRepo = new Mock<ICallSessionRepository>();
        _mockConvRepo = new Mock<IConversationRepository>();
        _handler = new InitiateCallCommandHandler(_mockCallRepo.Object, _mockConvRepo.Object);
    }

    /// <summary>
    /// Xác nhận request hợp lệ tạo call session trạng thái Requested.
    /// Luồng này kiểm tra mapping type và initiator khi chưa có active call.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_CreatesRequestedSession()
    {
        var conversationId = "conv-123";
        var initiatorId = Guid.NewGuid();
        var conv = new ConversationDto { Id = conversationId, Status = "ongoing", UserId = initiatorId.ToString() };

        _mockConvRepo.Setup(r => r.GetByIdAsync(conversationId, default)).ReturnsAsync(conv);
        _mockCallRepo.Setup(r => r.GetActiveByConversationAsync(conversationId, default)).ReturnsAsync((CallSessionDto)null!);

        // Gọi handler và assert call session được tạo đúng trạng thái/type.
        var command = new InitiateCallCommand { ConversationId = conversationId, InitiatorId = initiatorId, Type = "video" };
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(CallSessionStatus.Requested, result.Status);
        Assert.Equal(CallType.Video, result.Type);
        Assert.Equal(initiatorId.ToString(), result.InitiatorId);

        _mockCallRepo.Verify(x => x.AddAsync(It.IsAny<CallSessionDto>(), default), Times.Once);
    }

    /// <summary>
    /// Xác nhận hội thoại không ở trạng thái ongoing thì bị từ chối.
    /// Luồng này bảo vệ rule chỉ cho phép gọi khi conversation đang hoạt động.
    /// </summary>
    [Fact]
    public async Task Handle_ConversationNotOngoing_ThrowsBadRequest()
    {
        var conversationId = "conv-123";
        var initiatorId = Guid.NewGuid();

        var conv = new ConversationDto { Id = conversationId, Status = "pending", UserId = initiatorId.ToString() };

        _mockConvRepo.Setup(r => r.GetByIdAsync(conversationId, default)).ReturnsAsync(conv);

        var command = new InitiateCallCommand { ConversationId = conversationId, InitiatorId = initiatorId, Type = "audio" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận người không thuộc hội thoại không thể khởi tạo cuộc gọi.
    /// Luồng này đảm bảo kiểm soát quyền participant trước khi tạo call session.
    /// </summary>
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

    /// <summary>
    /// Xác nhận khi đã có active call thì không cho tạo thêm call mới.
    /// Luồng này tránh trùng phiên gọi trong cùng hội thoại.
    /// </summary>
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
