using Moq;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Profile.Commands.PresignAvatarUpload;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Profile.Commands;

public class PresignAvatarUploadCommandExecutorTests
{
    private readonly Mock<IR2UploadService> _r2UploadServiceMock = new();
    private readonly Mock<IUploadSessionRepository> _uploadSessionRepositoryMock = new();

    [Fact]
    public async Task Handle_ValidRequest_ReturnsPresignedUrl_AndStoresSession()
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

        var handler = new PresignAvatarUploadCommandExecutor(_r2UploadServiceMock.Object, _uploadSessionRepositoryMock.Object);
        var result = await handler.Handle(new PresignAvatarUploadCommand
        {
            UserId = userId,
            ContentType = "image/webp",
            SizeBytes = 120_000,
        }, CancellationToken.None);

        Assert.StartsWith("avatars/", result.ObjectKey);
        Assert.EndsWith(".webp", result.ObjectKey);
        Assert.Equal("https://r2.test.local/presigned", result.UploadUrl);
        Assert.Equal($"https://media.test.local/{result.ObjectKey}", result.PublicUrl);
        Assert.False(string.IsNullOrWhiteSpace(result.UploadToken));

        Assert.NotNull(capturedSession);
        Assert.Equal(MediaUploadConstants.ScopeAvatar, capturedSession!.Scope);
        Assert.Equal(userId, capturedSession.OwnerUserId);
        Assert.Equal(result.ObjectKey, capturedSession.ObjectKey);
        Assert.Equal(result.PublicUrl, capturedSession.PublicUrl);
        Assert.Equal(MediaUploadConstants.RequiredImageMimeType, capturedSession.ContentType);
    }

    [Fact]
    public async Task Handle_InvalidContentType_ThrowsBadRequestException()
    {
        _r2UploadServiceMock.SetupGet(x => x.IsEnabled).Returns(true);
        var handler = new PresignAvatarUploadCommandExecutor(_r2UploadServiceMock.Object, _uploadSessionRepositoryMock.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(new PresignAvatarUploadCommand
        {
            UserId = Guid.NewGuid(),
            ContentType = "image/png",
            SizeBytes = 10_000,
        }, CancellationToken.None));

        _uploadSessionRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<UploadSessionRecord>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
