using Moq;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.CreateConversation;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

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

    [Fact]
    public async Task Handle_SameUserAndReader_ThrowsBadRequest()
    {
        var id = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = id, ReaderId = id };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_OfflineReader_ThrowsBadRequest()
    {
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = Guid.NewGuid(), ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.Offline };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ExistingConversation_ReturnsExisting()
    {
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = userId, ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.AcceptingQuestions };
        var existing = new ConversationDto { Id = "existing123" };
        
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);
        _mockConvRepo.Setup(x => x.GetActiveByParticipantsAsync(userId.ToString(), readerId.ToString(), default)).ReturnsAsync(existing);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal("existing123", result.Id);
        _mockConvRepo.Verify(x => x.AddAsync(It.IsAny<ConversationDto>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesPendingConversation()
    {
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = userId, ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.AcceptingQuestions };
        
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);
        _mockConvRepo.Setup(x => x.GetActiveByParticipantsAsync(userId.ToString(), readerId.ToString(), default)).ReturnsAsync((ConversationDto)null!);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(ConversationStatus.Pending, result.Status);
        Assert.Equal(userId.ToString(), result.UserId);
        Assert.Equal(readerId.ToString(), result.ReaderId);
        Assert.NotNull(result.OfferExpiresAt);
        _mockConvRepo.Verify(x => x.AddAsync(It.Is<ConversationDto>(c => c.Status == ConversationStatus.Pending && c.OfferExpiresAt != null), default), Times.Once);
    }

    /// <summary>
    /// TEST CASE: Reader profile không tồn tại → NotFoundException.
    ///
    /// Khi nào xảy ra?
    /// → ReaderId sai hoặc reader chưa được approved (chưa có profile trong MongoDB).
    /// → User cố chat với readerId giả.
    /// </summary>
    [Fact]
    public async Task Handle_ReaderProfileNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new CreateConversationCommand
        {
            UserId = Guid.NewGuid(),
            ReaderId = Guid.NewGuid()
        };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.ReaderId.ToString(), default))
            .ReturnsAsync((ReaderProfileDto)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// TEST CASE: Reader status = Online (nhưng chưa accepting) → bị chặn.
    ///
    /// Tại sao test case này quan trọng?
    /// → Rule mới chỉ cho phép tạo conversation khi reader đang AcceptingQuestions.
    /// </summary>
    [Fact]
    public async Task Handle_ReaderOnlineNotAccepting_ThrowsBadRequest()
    {
        // Arrange — reader online (không phải accepting)
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var command = new CreateConversationCommand { UserId = userId, ReaderId = readerId };
        var profile = new ReaderProfileDto { Status = ReaderOnlineStatus.Online };

        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(readerId.ToString(), default)).ReturnsAsync(profile);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        _mockConvRepo.Verify(x => x.AddAsync(It.IsAny<ConversationDto>(), default), Times.Never);
    }
}
