using Moq;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community.Commands.PresignCommunityImage;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Community.Commands;

public class PresignCommunityImageCommandExecutorTests
{
    private readonly Mock<IR2UploadService> _r2UploadServiceMock = new();
    private readonly Mock<IUploadSessionRepository> _uploadSessionRepositoryMock = new();

    [Fact]
    public async Task Handle_ValidRequest_CreatesCommunityUploadSession()
    {
        var userId = Guid.NewGuid();
        UploadSessionRecord? capturedSession = null;

        _r2UploadServiceMock.SetupGet(x => x.IsEnabled).Returns(true);
        _r2UploadServiceMock
            .Setup(x => x.BuildPublicUrl(It.IsAny<string>()))
            .Returns<string>(key => $"https://media.test.local/{key}");
        _r2UploadServiceMock
            .Setup(x => x.GeneratePresignedPutUrlAsync(It.IsAny<string>(), MediaUploadConstants.RequiredImageMimeType, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://r2.test.local/presigned");
        _uploadSessionRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<UploadSessionRecord>(), It.IsAny<CancellationToken>()))
            .Callback<UploadSessionRecord, CancellationToken>((session, _) => capturedSession = session)
            .Returns(Task.CompletedTask);

        var handler = new PresignCommunityImageCommandExecutor(_r2UploadServiceMock.Object, _uploadSessionRepositoryMock.Object);
        var result = await handler.Handle(new PresignCommunityImageCommand
        {
            UserId = userId,
            ContextType = "comment",
            ContextDraftId = "draft-1",
            ContentType = "image/webp",
            SizeBytes = 256_000,
        }, CancellationToken.None);

        Assert.StartsWith("community/comment/", result.ObjectKey);
        Assert.False(string.IsNullOrWhiteSpace(result.UploadToken));

        Assert.NotNull(capturedSession);
        Assert.Equal(MediaUploadConstants.ScopeCommunityImage, capturedSession!.Scope);
        Assert.Equal("comment", capturedSession.ContextType);
        Assert.Equal("draft-1", capturedSession.ContextDraftId);
    }

    [Fact]
    public async Task Handle_InvalidContextType_ThrowsBadRequestException()
    {
        _r2UploadServiceMock.SetupGet(x => x.IsEnabled).Returns(true);
        var handler = new PresignCommunityImageCommandExecutor(_r2UploadServiceMock.Object, _uploadSessionRepositoryMock.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(new PresignCommunityImageCommand
        {
            UserId = Guid.NewGuid(),
            ContextType = "profile",
            ContextDraftId = "draft-1",
            ContentType = "image/webp",
            SizeBytes = 1000,
        }, CancellationToken.None));
    }
}
