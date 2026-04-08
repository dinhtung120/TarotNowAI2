

using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.CreateConversation;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

// Unit test cho handler tạo hội thoại giữa user và reader.
public class CreateConversationCommandHandlerTests
{
    // Mock conversation repo để kiểm tra nhánh tạo mới/trả về hội thoại hiện có.
    private readonly Mock<IConversationRepository> _mockConvRepo;
    // Mock profile repo để xác thực trạng thái reader trước khi tạo conversation.
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    // Handler cần kiểm thử.
    private readonly CreateConversationCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho CreateConversationCommandHandler.
    /// Luồng dùng mock repositories để test độc lập logic nghiệp vụ chat.
    /// </summary>
    public CreateConversationCommandHandlerTests()
    {
        _mockConvRepo = new Mock<IConversationRepository>();
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _handler = new CreateConversationCommandHandler(_mockConvRepo.Object, _mockProfileRepo.Object);
    }

    /// <summary>
    /// Xác nhận user không thể tự tạo conversation với chính mình làm reader.
    /// Luồng này bảo vệ rule đầu vào cơ bản của nghiệp vụ chat.
    /// </summary>
    [Fact]
    public async Task Handle_SameUserAndReader_ThrowsBadRequest()
    {
        var id = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = id, ReaderId = id };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận reader offline vẫn tạo conversation trạng thái Pending.
    /// Luồng này đảm bảo user vẫn có thể gửi yêu cầu chờ reader phản hồi.
    /// </summary>
    [Fact]
    public async Task Handle_OfflineReader_StillCreatesPending()
    {
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = Guid.NewGuid(), ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.Offline };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);
        _mockConvRepo.Setup(x => x.GetActiveByParticipantsAsync(command.UserId.ToString(), readerId.ToString(), default))
            .ReturnsAsync((ConversationDto?)null);

        // Không có conversation active thì handler phải tạo mới ở trạng thái Pending.
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(ConversationStatus.Pending, result.Status);
    }

    /// <summary>
    /// Xác nhận khi đã có conversation active thì trả lại conversation đó.
    /// Luồng này tránh tạo trùng cuộc hội thoại giữa cùng cặp user-reader.
    /// </summary>
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
        _mockConvRepo.Verify(x => x.AddAsync(It.IsAny<ConversationDto>(), default), Times.Never);
    }

    /// <summary>
    /// Xác nhận request hợp lệ sẽ tạo conversation Pending với dữ liệu participant đúng.
    /// Luồng này kiểm tra cả status, participant fields và OfferExpiresAt mặc định.
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

    /// <summary>
    /// Xác nhận thiếu reader profile sẽ ném NotFoundException.
    /// Luồng này bảo vệ dữ liệu tham chiếu trước khi tạo conversation mới.
    /// </summary>
    [Fact]
    public async Task Handle_ReaderProfileNotFound_ThrowsNotFoundException()
    {
        var command = new CreateConversationCommand { UserId = Guid.NewGuid(), ReaderId = Guid.NewGuid() };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.ReaderId.ToString(), default))
            .ReturnsAsync((ReaderProfileDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận reader busy vẫn cho tạo conversation Pending.
    /// Luồng này giữ hành vi nhất quán với offline: cho phép gửi yêu cầu chờ.
    /// </summary>
    [Fact]
    public async Task Handle_ReaderBusy_StillCreatesPending()
    {
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = userId, ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.Busy };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);
        _mockConvRepo.Setup(x => x.GetActiveByParticipantsAsync(userId.ToString(), readerId.ToString(), default))
            .ReturnsAsync((ConversationDto?)null);

        // Dù reader busy, handler vẫn tạo pending conversation để xử lý bất đồng bộ.
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(ConversationStatus.Pending, result.Status);
        _mockConvRepo.Verify(x => x.AddAsync(It.IsAny<ConversationDto>(), default), Times.Once);
    }
}
