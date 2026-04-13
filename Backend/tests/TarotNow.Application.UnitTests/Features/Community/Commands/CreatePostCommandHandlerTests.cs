

using FluentAssertions;
using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community.Commands.CreatePost;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Community.Commands;

// Unit test cho handler tạo bài viết cộng đồng.
public class CreatePostCommandHandlerTests
{
    // Mock post repo để xác nhận thao tác tạo post.
    private readonly Mock<ICommunityPostRepository> _postRepoMock;
    // Mock user repo để kiểm tra tác giả tồn tại.
    private readonly Mock<IUserRepository> _userRepoMock;
    // Mock gamification service để kiểm tra side-effect cộng điểm.
    private readonly Mock<IGamificationService> _gamificationServiceMock;
    // Mock media attachment service để map ảnh markdown theo context draft.
    private readonly Mock<ICommunityMediaAttachmentService> _communityMediaAttachmentServiceMock;
    // Handler cần kiểm thử.
    private readonly CreatePostCommandHandler _handler;

    /// <summary>
    /// Khởi tạo fixture cho CreatePostCommandHandler.
    /// Luồng setup gamification no-op giúp test tập trung vào logic tạo post.
    /// </summary>
    public CreatePostCommandHandlerTests()
    {
        _postRepoMock = new Mock<ICommunityPostRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _gamificationServiceMock = new Mock<IGamificationService>();
        _communityMediaAttachmentServiceMock = new Mock<ICommunityMediaAttachmentService>();
        _gamificationServiceMock
            .Setup(x => x.OnPostCreatedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _communityMediaAttachmentServiceMock
            .Setup(x => x.AttachForNewEntityAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _handler = new CreatePostCommandHandler(
            _postRepoMock.Object,
            _userRepoMock.Object,
            _gamificationServiceMock.Object,
            _communityMediaAttachmentServiceMock.Object);
    }

    /// <summary>
    /// Xác nhận request hợp lệ sẽ tạo post thành công và gọi gamification.
    /// Luồng kiểm tra dữ liệu post đầu ra được map đúng từ request + user.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_CreatesPostSuccessfully()
    {
        var request = new CreatePostCommand
        {
            AuthorId = Guid.NewGuid(),
            Content = "Hello World!",
            Visibility = PostVisibility.Public
        };

        var user = new TarotNow.Domain.Entities.User("email@test.com", "username", "hash", "Alice", DateTime.UtcNow, true);

        _userRepoMock.Setup(x => x.GetByIdAsync(request.AuthorId, default)).ReturnsAsync(user);

        _postRepoMock.Setup(x => x.CreateAsync(It.IsAny<CommunityPostDto>(), default))
            .ReturnsAsync((CommunityPostDto p, CancellationToken _) =>
            {
                p.Id = "new_id";
                return p;
            });

        // Gọi handler và assert dữ liệu trả về khớp input kỳ vọng.
        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be("new_id");
        result.AuthorDisplayName.Should().Be("Alice");
        result.Content.Should().Be("Hello World!");
        result.Visibility.Should().Be(PostVisibility.Public);
        _gamificationServiceMock.Verify(x => x.OnPostCreatedAsync(request.AuthorId, It.IsAny<CancellationToken>()), Times.Once);
        _communityMediaAttachmentServiceMock.Verify(
            x => x.AttachForNewEntityAsync(
                request.AuthorId,
                "post",
                request.ContextDraftId,
                result.Id,
                result.Content,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Xác nhận nội dung rỗng bị từ chối.
    /// Luồng này bảo vệ chất lượng dữ liệu bài viết cộng đồng.
    /// </summary>
    [Fact]
    public async Task Handle_EmptyContent_ThrowsBadRequestException()
    {
        var request = new CreatePostCommand { AuthorId = Guid.NewGuid(), Content = "" };

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận visibility không hợp lệ bị từ chối.
    /// Luồng này bảo vệ business rule whitelist mức hiển thị bài viết.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidVisibility_ThrowsBadRequestException()
    {
        var request = new CreatePostCommand { AuthorId = Guid.NewGuid(), Content = "Hello", Visibility = "invalid" };

        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    }

    /// <summary>
    /// Xác nhận tác giả không tồn tại sẽ ném NotFoundException.
    /// Luồng này ngăn tạo post mồ côi không gắn user hợp lệ.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        var request = new CreatePostCommand { AuthorId = Guid.NewGuid(), Content = "Hello", Visibility = PostVisibility.Public };
        _userRepoMock.Setup(x => x.GetByIdAsync(request.AuthorId, default)).ReturnsAsync((TarotNow.Domain.Entities.User?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }
}
