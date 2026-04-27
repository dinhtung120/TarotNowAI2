using Moq;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community.Commands.ConfirmCommunityImage;
using TarotNow.Application.Interfaces;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Community.Commands;

public class ConfirmCommunityImageCommandHandlerRequestedDomainEventHandlerTests
{
    private readonly Mock<IUploadSessionRepository> _uploadSessionRepositoryMock = new();
    private readonly Mock<ICommunityMediaAssetRepository> _communityMediaAssetRepositoryMock = new();
    private readonly Mock<IR2UploadService> _r2UploadServiceMock = new();

    [Fact]
    public async Task Handle_ValidRequest_ConsumesToken_AndUpsertsAsset()
    {
        var userId = Guid.NewGuid();
        CommunityMediaAssetRecord? capturedAsset = null;

        _r2UploadServiceMock.SetupGet(x => x.IsEnabled).Returns(true);
        _uploadSessionRepositoryMock
            .Setup(x => x.GetByTokenAsync("token-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UploadSessionRecord
            {
                UploadToken = "token-1",
                OwnerUserId = userId,
                Scope = MediaUploadConstants.ScopeCommunityImage,
                ObjectKey = "community/post/file.webp",
                PublicUrl = "https://media.test.local/community/post/file.webp",
                ContextType = "post",
                ContextDraftId = "draft-post-1",
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(5),
            });
        _uploadSessionRepositoryMock
            .Setup(x => x.ConsumeAsync("token-1", It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _r2UploadServiceMock
            .Setup(x => x.ExistsObjectAsync("community/post/file.webp", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _communityMediaAssetRepositoryMock
            .Setup(x => x.UpsertUploadedAsync(It.IsAny<CommunityMediaAssetRecord>(), It.IsAny<CancellationToken>()))
            .Callback<CommunityMediaAssetRecord, CancellationToken>((asset, _) => capturedAsset = asset)
            .Returns(Task.CompletedTask);

        var handler = new ConfirmCommunityImageCommandHandlerRequestedDomainEventHandler(
            _uploadSessionRepositoryMock.Object,
            _communityMediaAssetRepositoryMock.Object,
            _r2UploadServiceMock.Object,
            Mock.Of<TarotNow.Application.Interfaces.DomainEvents.IEventHandlerIdempotencyService>());

        var result = await handler.Handle(new ConfirmCommunityImageCommand
        {
            UserId = userId,
            ContextType = "post",
            ContextDraftId = "draft-post-1",
            ObjectKey = "community/post/file.webp",
            PublicUrl = "https://media.test.local/community/post/file.webp",
            UploadToken = "token-1",
        }, CancellationToken.None);

        Assert.Equal("community/post/file.webp", result.ObjectKey);
        Assert.Equal("post", result.ContextType);
        Assert.NotNull(capturedAsset);
        Assert.Equal(MediaUploadConstants.AssetStatusUploaded, capturedAsset!.Status);
        Assert.Equal(userId, capturedAsset.OwnerUserId);
        Assert.Equal("draft-post-1", capturedAsset.ContextDraftId);
    }

    [Fact]
    public async Task Handle_ObjectKeyMismatch_ThrowsBadRequestException()
    {
        var userId = Guid.NewGuid();
        _r2UploadServiceMock.SetupGet(x => x.IsEnabled).Returns(true);
        _uploadSessionRepositoryMock
            .Setup(x => x.GetByTokenAsync("token-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UploadSessionRecord
            {
                UploadToken = "token-1",
                OwnerUserId = userId,
                Scope = MediaUploadConstants.ScopeCommunityImage,
                ObjectKey = "community/post/right.webp",
                PublicUrl = "https://media.test.local/community/post/right.webp",
                ContextType = "post",
                ContextDraftId = "draft-post-1",
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(5),
            });

        var handler = new ConfirmCommunityImageCommandHandlerRequestedDomainEventHandler(
            _uploadSessionRepositoryMock.Object,
            _communityMediaAssetRepositoryMock.Object,
            _r2UploadServiceMock.Object,
            Mock.Of<TarotNow.Application.Interfaces.DomainEvents.IEventHandlerIdempotencyService>());

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(new ConfirmCommunityImageCommand
        {
            UserId = userId,
            ContextType = "post",
            ContextDraftId = "draft-post-1",
            ObjectKey = "community/post/wrong.webp",
            PublicUrl = "https://media.test.local/community/post/right.webp",
            UploadToken = "token-1",
        }, CancellationToken.None));

        _communityMediaAssetRepositoryMock.Verify(x => x.UpsertUploadedAsync(It.IsAny<CommunityMediaAssetRecord>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
