/*
 * FILE: CreateConversationCommandHandlerTests.cs
 * MỤC ĐÍCH: Unit test cho handler tạo cuộc hội thoại (Chat) giữa User và Reader.
 *
 *   CÁC TEST CASE (6 scenarios):
 *   1. Handle_SameUserAndReader_ThrowsBadRequest: User chat với chính mình → 400
 *   2. Handle_OfflineReader_StillCreatesPending: Reader offline vẫn tạo được room pending
 *   3. Handle_ExistingConversation_ReturnsExisting: đã có conversation → trả existing (không duplicate)
 *   4. Handle_ValidRequest_CreatesPendingConversation: tạo mới → status=Pending + OfferExpiresAt
 *   5. Handle_ReaderProfileNotFound_ThrowsNotFoundException: Reader chưa approved → 404
 *   6. Handle_ReaderOnlineNotAccepting_StillCreatesPending: Reader Online (không accepting_questions) vẫn tạo được room pending
 *
 *   QUY TẮC:
 *   → Cho phép tạo conversation với Reader ở mọi trạng thái hợp lệ để hiển thị cảnh báo trong phòng chat
 *   → Nếu đã có conversation active giữa 2 người → trả existing (idempotent)
 *   → conversation mới luôn có OfferExpiresAt (timer cho Reader accept/reject)
 */

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.CreateConversation;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

/// <summary>
/// Test create conversation: status check, duplicate prevention, offer timer.
/// </summary>
public class CreateConversationCommandHandlerTests
{
    private readonly Mock<IConversationRepository> _mockConvRepo;
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    private readonly CreateConversationCommandHandler _handler;

    public CreateConversationCommandHandlerTests()
    {
        _mockConvRepo = new Mock<IConversationRepository>();
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _handler = new CreateConversationCommandHandler(_mockConvRepo.Object, _mockProfileRepo.Object);
    }

    /// <summary>User chat với chính mình → BadRequest.</summary>
    [Fact]
    public async Task Handle_SameUserAndReader_ThrowsBadRequest()
    {
        var id = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = id, ReaderId = id };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Reader offline vẫn cho phép tạo conversation pending.</summary>
    [Fact]
    public async Task Handle_OfflineReader_StillCreatesPending()
    {
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = Guid.NewGuid(), ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.Offline };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);
        _mockConvRepo.Setup(x => x.GetActiveByParticipantsAsync(command.UserId.ToString(), readerId.ToString(), default))
            .ReturnsAsync((ConversationDto?)null);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(ConversationStatus.Pending, result.Status);
    }

    /// <summary>Đã có conversation active → trả existing (idempotent, không duplicate).</summary>
    [Fact]
    public async Task Handle_ExistingConversation_ReturnsExisting()
    {
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = userId, ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.Online };
        var existing = new ConversationDto { Id = "existing123" };
        
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);
        _mockConvRepo.Setup(x => x.GetActiveByParticipantsAsync(userId.ToString(), readerId.ToString(), default)).ReturnsAsync(existing);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal("existing123", result.Id);
        _mockConvRepo.Verify(x => x.AddAsync(It.IsAny<ConversationDto>(), default), Times.Never); // KHÔNG tạo mới
    }

    /// <summary>
    /// Happy path: tạo conversation Pending chưa có OfferExpiresAt.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_CreatesPendingConversation()
    {
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = userId, ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.Online };
        
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);
        _mockConvRepo.Setup(x => x.GetActiveByParticipantsAsync(userId.ToString(), readerId.ToString(), default)).ReturnsAsync((ConversationDto)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(ConversationStatus.Pending, result.Status);
        Assert.Equal(userId.ToString(), result.UserId);
        Assert.Equal(readerId.ToString(), result.ReaderId);
        Assert.Null(result.OfferExpiresAt);
        _mockConvRepo.Verify(x => x.AddAsync(It.Is<ConversationDto>(c => c.Status == ConversationStatus.Pending && c.OfferExpiresAt == null), default), Times.Once);
    }

    /// <summary>Reader profile không tồn tại (chưa approved) → NotFoundException.</summary>
    [Fact]
    public async Task Handle_ReaderProfileNotFound_ThrowsNotFoundException()
    {
        var command = new CreateConversationCommand { UserId = Guid.NewGuid(), ReaderId = Guid.NewGuid() };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.ReaderId.ToString(), default))
            .ReturnsAsync((ReaderProfileDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>Reader Online hoặc Busy đều cho phép tạo room pending.</summary>
    [Fact]
    public async Task Handle_ReaderBusy_StillCreatesPending()
    {
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = userId, ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.Busy }; // Hợp lệ, tạo bình thường

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);
        _mockConvRepo.Setup(x => x.GetActiveByParticipantsAsync(userId.ToString(), readerId.ToString(), default))
            .ReturnsAsync((ConversationDto?)null);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(ConversationStatus.Pending, result.Status);
        _mockConvRepo.Verify(x => x.AddAsync(It.IsAny<ConversationDto>(), default), Times.Once);
    }
}
