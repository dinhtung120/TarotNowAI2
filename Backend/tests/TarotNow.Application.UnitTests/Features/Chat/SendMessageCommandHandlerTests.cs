using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat;

// Unit test cho handler gửi tin nhắn chat với luồng media uploadToken one-time.
public class SendMessageCommandHandlerTests
{
    private readonly Mock<IConversationRepository> _mockConvRepo;
    private readonly Mock<IChatMessageRepository> _mockMsgRepo;
    private readonly Mock<IChatFinanceRepository> _mockFinanceRepo;
    private readonly Mock<IWalletRepository> _mockWalletRepo;
    private readonly Mock<IReaderProfileRepository> _mockReaderProfileRepo;
    private readonly Mock<ITransactionCoordinator> _mockTransactionCoordinator;
    private readonly Mock<IUploadSessionRepository> _mockUploadSessionRepository;
    private readonly Mock<IDomainEventPublisher> _mockDomainEventPublisher;
    private readonly SendMessageCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho SendMessageCommandHandler.
    /// </summary>
    public SendMessageCommandHandlerTests()
    {
        _mockConvRepo = new Mock<IConversationRepository>();
        _mockMsgRepo = new Mock<IChatMessageRepository>();
        _mockFinanceRepo = new Mock<IChatFinanceRepository>();
        _mockWalletRepo = new Mock<IWalletRepository>();
        _mockReaderProfileRepo = new Mock<IReaderProfileRepository>();
        _mockTransactionCoordinator = new Mock<ITransactionCoordinator>();
        _mockUploadSessionRepository = new Mock<IUploadSessionRepository>();
        _mockDomainEventPublisher = new Mock<IDomainEventPublisher>();

        _mockTransactionCoordinator
            .Setup(x => x.ExecuteAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns((Func<CancellationToken, Task> action, CancellationToken token) => action(token));

        _handler = new SendMessageCommandHandler(
            _mockConvRepo.Object,
            _mockMsgRepo.Object,
            _mockFinanceRepo.Object,
            _mockWalletRepo.Object,
            _mockReaderProfileRepo.Object,
            _mockTransactionCoordinator.Object,
            _mockUploadSessionRepository.Object,
            _mockDomainEventPublisher.Object);
    }

    [Fact]
    public async Task Handle_InvalidType_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = "invalid_type", Content = "a" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_EmptyText_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NotAMember_ThrowsBadRequest()
    {
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "a", ConversationId = "c1", SenderId = Guid.NewGuid() };
        var conv = new ConversationDto { UserId = Guid.NewGuid().ToString(), ReaderId = Guid.NewGuid().ToString() };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidFirstUserMessage_PersistsAndFreezesWallet()
    {
        var senderId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var command = new SendMessageCommand { Type = ChatMessageType.Text, Content = "Hello", ConversationId = "c1", SenderId = senderId };
        var conv = new ConversationDto { Id = "c1", UserId = senderId.ToString(), ReaderId = readerId.ToString(), Status = ConversationStatus.Pending, UnreadCountReader = 0 };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);
        _mockReaderProfileRepo
            .Setup(x => x.GetByUserIdAsync(readerId.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReaderProfileDto
            {
                UserId = readerId.ToString(),
                Status = ReaderOnlineStatus.Online,
                DiamondPerQuestion = 10,
            });
        _mockFinanceRepo
            .Setup(x => x.GetItemByIdempotencyKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ChatQuestionItem?)null);
        _mockFinanceRepo
            .Setup(x => x.GetSessionByConversationRefAsync("c1", It.IsAny<CancellationToken>()))
            .ReturnsAsync((ChatFinanceSession?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(ChatMessageType.Text, result.Type);
        Assert.Equal("Hello", result.Content);
        Assert.Equal(ConversationStatus.AwaitingAcceptance, conv.Status);
        Assert.Equal(1, conv.UnreadCountReader);

        _mockMsgRepo.Verify(x => x.AddAsync(It.IsAny<ChatMessageDto>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        _mockConvRepo.Verify(x => x.UpdateAsync(conv, default), Times.Once);
        _mockWalletRepo.Verify(
            x => x.FreezeAsync(
                It.IsAny<Guid>(),
                10,
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ConversationNotFound_ThrowsNotFoundException()
    {
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text,
            Content = "Hello",
            ConversationId = "non_existent",
            SenderId = Guid.NewGuid(),
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("non_existent", default)).ReturnsAsync((ConversationDto)null!);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_CompletedConversation_ThrowsBadRequest()
    {
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text,
            Content = "Hello",
            ConversationId = "c1",
            SenderId = senderId,
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Completed,
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        _mockMsgRepo.Verify(x => x.AddAsync(It.IsAny<ChatMessageDto>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_ReaderSendsMessage_UserUnreadCountIncreases()
    {
        var userId = Guid.NewGuid();
        var readerId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Text,
            Content = "Reader reply",
            ConversationId = "c1",
            SenderId = readerId,
        };
        var conv = new ConversationDto
        {
            Id = "c1",
            UserId = userId.ToString(),
            ReaderId = readerId.ToString(),
            Status = ConversationStatus.Ongoing,
            UnreadCountUser = 0,
            UnreadCountReader = 0,
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(conv);
        _mockFinanceRepo
            .Setup(x => x.GetSessionByConversationRefAsync("c1", It.IsAny<CancellationToken>()))
            .ReturnsAsync((ChatFinanceSession?)null);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(1, conv.UnreadCountUser);
        Assert.Equal(0, conv.UnreadCountReader);
    }

    [Fact]
    public async Task Handle_ImageMessageWithoutPayload_ThrowsBadRequest()
    {
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Image,
            ConversationId = "c1",
            SenderId = senderId,
            Content = "",
            MediaPayload = null,
        };
        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing,
        });

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ImageMessageWithValidUploadSession_SucceedsAndConsumesToken()
    {
        var senderId = Guid.NewGuid();
        var token = "upload-token";
        var mediaUrl = "https://media.test.local/chat/c1/images/file.webp";
        var objectKey = "chat/c1/images/file.webp";

        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Image,
            ConversationId = "c1",
            SenderId = senderId,
            MediaPayload = new MediaPayloadDto
            {
                Url = mediaUrl,
                ObjectKey = objectKey,
                UploadToken = token,
                MimeType = "image/webp",
                SizeBytes = 128_000,
            },
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing,
        });

        _mockUploadSessionRepository
            .Setup(x => x.GetByTokenAsync(token, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UploadSessionRecord
            {
                UploadToken = token,
                OwnerUserId = senderId,
                Scope = MediaUploadConstants.ScopeChatImage,
                ObjectKey = objectKey,
                PublicUrl = mediaUrl,
                ContentType = "image/webp",
                ConversationId = "c1",
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(5),
            });
        _mockUploadSessionRepository
            .Setup(x => x.ConsumeAsync(token, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(ChatMessageType.Image, result.Type);
        Assert.Equal("[image]", result.Content);
        Assert.Equal("image/webp", result.MediaPayload!.MimeType);
        Assert.Null(result.MediaPayload!.UploadToken);

        _mockUploadSessionRepository.Verify(x => x.ConsumeAsync(token, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ImageMessageWithMismatchedToken_ThrowsBadRequest()
    {
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Image,
            ConversationId = "c1",
            SenderId = senderId,
            MediaPayload = new MediaPayloadDto
            {
                Url = "https://media.test.local/chat/c1/images/file.webp",
                ObjectKey = "chat/c1/images/file.webp",
                UploadToken = "token-1",
                MimeType = "image/webp",
                SizeBytes = 1024,
            },
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing,
        });

        _mockUploadSessionRepository
            .Setup(x => x.GetByTokenAsync("token-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UploadSessionRecord
            {
                UploadToken = "token-1",
                OwnerUserId = senderId,
                Scope = MediaUploadConstants.ScopeChatVoice,
                ObjectKey = "chat/c1/voices/file.webm",
                PublicUrl = "https://media.test.local/chat/c1/voices/file.webm",
                ContentType = "audio/webm",
                ConversationId = "c1",
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(5),
            });

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
        _mockUploadSessionRepository.Verify(x => x.ConsumeAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_VoiceMessageDurationTooLong_ThrowsBadRequest()
    {
        var senderId = Guid.NewGuid();
        var command = new SendMessageCommand
        {
            Type = ChatMessageType.Voice,
            ConversationId = "c1",
            SenderId = senderId,
            MediaPayload = new MediaPayloadDto
            {
                Url = "https://media.test.local/chat/c1/voices/file.webm",
                ObjectKey = "chat/c1/voices/file.webm",
                UploadToken = "token-voice",
                MimeType = "audio/webm",
                SizeBytes = 1024,
                DurationMs = 700_000,
            },
        };

        _mockConvRepo.Setup(x => x.GetByIdAsync("c1", default)).ReturnsAsync(new ConversationDto
        {
            Id = "c1",
            UserId = senderId.ToString(),
            ReaderId = Guid.NewGuid().ToString(),
            Status = ConversationStatus.Ongoing,
        });

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
