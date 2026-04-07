

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

        [Fact]
    public async Task Handle_ReaderProfileNotFound_ThrowsNotFoundException()
    {
        var command = new CreateConversationCommand { UserId = Guid.NewGuid(), ReaderId = Guid.NewGuid() };
        _mockProfileRepo.Setup(x => x.GetByUserIdAsync(command.ReaderId.ToString(), default))
            .ReturnsAsync((ReaderProfileDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

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

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Equal(ConversationStatus.Pending, result.Status);
        _mockConvRepo.Verify(x => x.AddAsync(It.IsAny<ConversationDto>(), default), Times.Once);
    }
}
