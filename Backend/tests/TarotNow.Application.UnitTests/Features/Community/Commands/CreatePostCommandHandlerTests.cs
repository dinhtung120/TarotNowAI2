/*
 * ===================================================================
 * FILE: CreatePostCommandHandlerTests.cs
 * NAMESPACE: TarotNow.Application.UnitTests.Features.Community.Commands
 * ===================================================================
 */

using FluentAssertions;
using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community.Commands.CreatePost;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Community.Commands;

public class CreatePostCommandHandlerTests
{
    private readonly Mock<ICommunityPostRepository> _postRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly CreatePostCommandHandler _handler;

    public CreatePostCommandHandlerTests()
    {
        _postRepoMock = new Mock<ICommunityPostRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _handler = new CreatePostCommandHandler(_postRepoMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesPostSuccessfully()
    {
        // Arrange
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

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("new_id");
        result.AuthorDisplayName.Should().Be("Alice");
        result.Content.Should().Be("Hello World!");
        result.Visibility.Should().Be(PostVisibility.Public);
    }

    [Fact]
    public async Task Handle_EmptyContent_ThrowsBadRequestException()
    {
        // Arrange
        var request = new CreatePostCommand { AuthorId = Guid.NewGuid(), Content = "" };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidVisibility_ThrowsBadRequestException()
    {
        // Arrange
        var request = new CreatePostCommand { AuthorId = Guid.NewGuid(), Content = "Hello", Visibility = "invalid" };

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new CreatePostCommand { AuthorId = Guid.NewGuid(), Content = "Hello", Visibility = PostVisibility.Public };
        _userRepoMock.Setup(x => x.GetByIdAsync(request.AuthorId, default)).ReturnsAsync((TarotNow.Domain.Entities.User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }
}
