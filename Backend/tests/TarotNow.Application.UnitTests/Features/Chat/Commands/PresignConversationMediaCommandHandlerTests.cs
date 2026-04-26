using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Chat.Commands.PresignConversationMedia;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Chat.Commands;

public class PresignConversationMediaCommandExecutorTests
{
    private readonly Mock<IConversationRepository> _conversationRepositoryMock = new();
    private readonly Mock<IR2UploadService> _r2UploadServiceMock = new();
    private readonly Mock<IUploadSessionRepository> _uploadSessionRepositoryMock = new();

    [Fact]
    public async Task Handle_ValidImageRequest_ReturnsPresignedAndStoresUploadSession()
    {
        var requesterId = Guid.NewGuid();
        UploadSessionRecord? capturedSession = null;

        _r2UploadServiceMock.SetupGet(x => x.IsEnabled).Returns(true);
        _r2UploadServiceMock
            .Setup(x => x.BuildPublicUrl(It.IsAny<string>()))
            .Returns<string>(key => $"https://media.test.local/{key}");
        _r2UploadServiceMock
            .Setup(x => x.GeneratePresignedPutUrlAsync(It.IsAny<string>(), "image/webp", It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://r2.test.local/presigned");

        _conversationRepositoryMock
            .Setup(x => x.GetByIdAsync("c-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConversationDto
            {
                Id = "c-1",
                UserId = requesterId.ToString(),
                ReaderId = Guid.NewGuid().ToString(),
                Status = ConversationStatus.Ongoing,
            });

        _uploadSessionRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<UploadSessionRecord>(), It.IsAny<CancellationToken>()))
            .Callback<UploadSessionRecord, CancellationToken>((session, _) => capturedSession = session)
            .Returns(Task.CompletedTask);

        var handler = new PresignConversationMediaCommandExecutor(
            _conversationRepositoryMock.Object,
            _r2UploadServiceMock.Object,
            _uploadSessionRepositoryMock.Object);

        var result = await handler.Handle(new PresignConversationMediaCommand
        {
            ConversationId = "c-1",
            RequesterId = requesterId,
            MediaKind = "image",
            ContentType = "image/webp",
            SizeBytes = 200_000,
        }, CancellationToken.None);

        Assert.StartsWith("chat/c-1/images/", result.ObjectKey);
        Assert.Equal("https://r2.test.local/presigned", result.UploadUrl);
        Assert.NotNull(capturedSession);
        Assert.Equal(MediaUploadConstants.ScopeChatImage, capturedSession!.Scope);
        Assert.Equal("c-1", capturedSession.ConversationId);
    }

    [Fact]
    public async Task Handle_InvalidVoiceMimeType_ThrowsBadRequestException()
    {
        _r2UploadServiceMock.SetupGet(x => x.IsEnabled).Returns(true);
        var handler = new PresignConversationMediaCommandExecutor(
            _conversationRepositoryMock.Object,
            _r2UploadServiceMock.Object,
            _uploadSessionRepositoryMock.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(new PresignConversationMediaCommand
        {
            ConversationId = "c-1",
            RequesterId = Guid.NewGuid(),
            MediaKind = "voice",
            ContentType = "audio/flac",
            SizeBytes = 20_000,
            DurationMs = 5000,
        }, CancellationToken.None));
    }
}
