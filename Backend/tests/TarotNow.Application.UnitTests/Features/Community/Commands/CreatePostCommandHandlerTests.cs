using FluentAssertions;
using Moq;
using TarotNow.Application.Common.MediaUpload;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community.Commands.CreatePost;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Community.Commands;

// Unit test cho handler tạo bài viết cộng đồng.
public class CreatePostCommandHandlerTests
{
    // Mock dispatcher inline domain event cho flow command event-only.
    private readonly Mock<IInlineDomainEventDispatcher> _inlineDomainEventDispatcherMock;
    // Handler cần kiểm thử.
    private readonly CreatePostCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho CreatePostCommandHandler.
    /// </summary>
    public CreatePostCommandHandlerTests()
    {
        _inlineDomainEventDispatcherMock = new Mock<IInlineDomainEventDispatcher>();
        _handler = new CreatePostCommandHandler(_inlineDomainEventDispatcherMock.Object);
    }

    /// <summary>
    /// Xác nhận request hợp lệ sẽ publish event và map response từ payload event.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_MapsResultFromEventPayload()
    {
        var request = new CreatePostCommand
        {
            AuthorId = Guid.NewGuid(),
            Content = "Hello World!",
            Visibility = PostVisibility.Public
        };

        _inlineDomainEventDispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<CommunityPostCreateRequestedDomainEvent>(), It.IsAny<CancellationToken>()))
            .Callback((IDomainEvent evt, CancellationToken _) =>
            {
                var domainEvent = (CommunityPostCreateRequestedDomainEvent)evt;
                domainEvent.CreatedPostId = "new_id";
                domainEvent.AuthorDisplayName = "Alice";
                domainEvent.AuthorAvatarUrl = "https://example.com/a.webp";
                domainEvent.CreatedAtUtc = DateTime.UtcNow;
                domainEvent.MediaAttachStatus = MediaUploadConstants.EntityMediaAttachStatusPending;
            })
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be("new_id");
        result.AuthorDisplayName.Should().Be("Alice");
        result.Content.Should().Be("Hello World!");
        result.Visibility.Should().Be(PostVisibility.Public);
        result.MediaAttachStatus.Should().Be(MediaUploadConstants.EntityMediaAttachStatusPending);
        _inlineDomainEventDispatcherMock.Verify(
            x => x.PublishAsync(
                It.Is<CommunityPostCreateRequestedDomainEvent>(e =>
                    e.AuthorId == request.AuthorId
                    && e.Content == "Hello World!"
                    && e.Visibility == PostVisibility.Public),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Xác nhận nội dung rỗng bị từ chối.
    /// </summary>
    [Fact]
    public async Task Handle_EmptyContent_ThrowsBadRequestException()
    {
        var request = new CreatePostCommand { AuthorId = Guid.NewGuid(), Content = "" };

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận visibility không hợp lệ bị từ chối.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidVisibility_ThrowsBadRequestException()
    {
        var request = new CreatePostCommand { AuthorId = Guid.NewGuid(), Content = "Hello", Visibility = "invalid" };

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận nếu event payload không trả CreatedPostId thì command fail an toàn.
    /// </summary>
    [Fact]
    public async Task Handle_MissingCreatedPostId_ThrowsBadRequestException()
    {
        var request = new CreatePostCommand { AuthorId = Guid.NewGuid(), Content = "Hello", Visibility = PostVisibility.Public };
        _inlineDomainEventDispatcherMock
            .Setup(x => x.PublishAsync(It.IsAny<CommunityPostCreateRequestedDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    }
}
