using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Reader;

/// <summary>
/// Unit tests cho UpdateReaderStatusCommandHandler — chuyển đổi trạng thái online của Reader.
///
/// Tại sao handler này là core feature?
/// → Trạng thái reader là gate check cho toàn bộ chat flow (Phase 2.1 TEST):
///   - reader offline → reject chat request
///   - reader accepting_questions → cho phép tạo conversation
///
/// Nếu bug ở handler này:
/// → Reader set offline nhưng vẫn nhận chat → trải nghiệm user xấu
/// → Status invalid nhưng không reject → database corrupt
///
/// Trạng thái hợp lệ theo domain (ReaderOnlineStatus enum):
/// - "online" → reader online nhưng chưa muốn nhận câu hỏi mới
/// - "offline" → reader offline, KHÔNG hiện trên directory
/// - "accepting_questions" → reader sẵn sàng nhận câu hỏi mới
/// </summary>
public class UpdateReaderStatusCommandHandlerTests
{
    /* Mock reader profile repository (MongoDB) */
    private readonly Mock<IReaderProfileRepository> _mockProfileRepo;
    private readonly UpdateReaderStatusCommandHandler _handler;

    public UpdateReaderStatusCommandHandlerTests()
    {
        _mockProfileRepo = new Mock<IReaderProfileRepository>();
        _handler = new UpdateReaderStatusCommandHandler(_mockProfileRepo.Object);
    }

    /// <summary>
    /// TEST CASE: Trạng thái không hợp lệ → BadRequestException.
    ///
    /// Tại sao validate ở handler thay vì FluentValidation?
    /// → Status là domain concept (ReaderOnlineStatus enum).
    /// → Validate ở handler cho error message business-friendly (tiếng Việt).
    /// → Tránh magic strings leak vào validator layer.
    ///
    /// Ví dụ invalid: "busy", "away", "123", ""
    /// </summary>
    [Theory]
    [InlineData("busy")]
    [InlineData("away")]
    [InlineData("invalid")]
    [InlineData("")]
    public async Task Handle_InvalidStatus_ThrowsBadRequestException(string invalidStatus)
    {
        // Arrange
        var command = new UpdateReaderStatusCommand
        {
            UserId = Guid.NewGuid(),
            Status = invalidStatus
        };

        // Act & Assert — handler phải reject TRƯỚC khi gọi database
        var ex = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("không hợp lệ", ex.Message);

        // Verify — KHÔNG gọi database nếu status invalid (early return pattern)
        _mockProfileRepo.Verify(
            x => x.GetByUserIdAsync(It.IsAny<string>(), default), Times.Never);
    }

    /// <summary>
    /// TEST CASE: Profile không tồn tại → NotFoundException.
    ///
    /// Khi nào xảy ra?
    /// → User chưa được approve làm reader (chưa có profile trong MongoDB).
    /// → User thường cố gắng set status reader.
    /// </summary>
    [Fact]
    public async Task Handle_ProfileNotFound_ThrowsNotFoundException()
    {
        // Arrange — status hợp lệ nhưng profile không tồn tại
        var command = new UpdateReaderStatusCommand
        {
            UserId = Guid.NewGuid(),
            Status = ReaderOnlineStatus.Online
        };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.UserId.ToString(), default))
            .ReturnsAsync((ReaderProfileDto)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: Happy path — set status = accepting_questions thành công.
    ///
    /// Đây là trạng thái quan trọng nhất:
    /// → accepting_questions = reader sẵn sàng nhận chat mới
    /// → Directory listing chỉ hiện reader ở trạng thái này
    /// → Gate check: CreateConversation chỉ cho phép khi reader accepting_questions
    /// </summary>
    [Fact]
    public async Task Handle_ValidAcceptingStatus_UpdatesSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateReaderStatusCommand
        {
            UserId = userId,
            Status = ReaderOnlineStatus.AcceptingQuestions
        };
        var existingProfile = new ReaderProfileDto
        {
            UserId = userId.ToString(),
            Status = ReaderOnlineStatus.Offline // Đang offline
        };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default))
            .ReturnsAsync(existingProfile);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert — trả về true, status đã đổi
        Assert.True(result);
        Assert.Equal(ReaderOnlineStatus.AcceptingQuestions, existingProfile.Status);

        // Assert — UpdateAsync được gọi
        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }

    /// <summary>
    /// TEST CASE: Set status = offline thành công.
    ///
    /// Verify reader có thể tự set offline → sẽ bị ẩn khỏi directory.
    /// Gate check trong CreateConversation sẽ reject chat request khi reader offline.
    /// </summary>
    [Fact]
    public async Task Handle_ValidOfflineStatus_UpdatesSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateReaderStatusCommand
        {
            UserId = userId,
            Status = ReaderOnlineStatus.Offline
        };
        var existingProfile = new ReaderProfileDto
        {
            UserId = userId.ToString(),
            Status = ReaderOnlineStatus.AcceptingQuestions // Đang accepting
        };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(userId.ToString(), default))
            .ReturnsAsync(existingProfile);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Equal(ReaderOnlineStatus.Offline, existingProfile.Status);
        _mockProfileRepo.Verify(x => x.UpdateAsync(existingProfile, default), Times.Once);
    }
}
