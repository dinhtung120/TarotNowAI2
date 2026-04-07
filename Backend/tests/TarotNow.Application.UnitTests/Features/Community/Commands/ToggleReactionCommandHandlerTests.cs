

using Moq;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community.Commands.ToggleReaction;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using Xunit;

namespace TarotNow.Application.UnitTests.Features.Community.Commands;

public class ToggleReactionCommandHandlerTests
{
    private readonly Mock<ICommunityReactionRepository> _reactionRepoMock;
    private readonly Mock<ICommunityPostRepository> _postRepoMock;
    private readonly ToggleReactionCommandHandler _handler;

    public ToggleReactionCommandHandlerTests()
    {
        _reactionRepoMock = new Mock<ICommunityReactionRepository>();
        _postRepoMock = new Mock<ICommunityPostRepository>();
        _handler = new ToggleReactionCommandHandler(_reactionRepoMock.Object, _postRepoMock.Object);
    }

    [Fact]
    public async Task Handle_InvalidReactionType_ThrowsBadRequestException()
    {
        var request = new ToggleReactionCommand { PostId = "post1", UserId = Guid.NewGuid(), ReactionType = "invalid" };
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_PostNotFound_ThrowsNotFoundException()
    {
        var request = new ToggleReactionCommand { PostId = "post1", UserId = Guid.NewGuid(), ReactionType = ReactionType.Like };
        _postRepoMock.Setup(x => x.GetByIdAsync("post1", default)).ReturnsAsync((CommunityPostDto?)null);
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NewReaction_AddsAndIncrements()
    {
        var request = new ToggleReactionCommand { PostId = "post1", UserId = Guid.NewGuid(), ReactionType = ReactionType.Love };
        var post = new CommunityPostDto { Id = "post1" };
        
        _postRepoMock.Setup(x => x.GetByIdAsync("post1", default)).ReturnsAsync(post);
        _reactionRepoMock.Setup(x => x.GetAsync("post1", request.UserId.ToString(), default)).ReturnsAsync((CommunityReactionDto?)null);
        _reactionRepoMock.Setup(x => x.AddOrIgnoreAsync(It.IsAny<CommunityReactionDto>(), default)).ReturnsAsync(true);

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.True(result);
        _reactionRepoMock.Verify(x => x.AddOrIgnoreAsync(It.IsAny<CommunityReactionDto>(), default), Times.Once);
        _postRepoMock.Verify(x => x.IncrementReactionCountAsync("post1", ReactionType.Love, 1, default), Times.Once);
    }

    [Fact]
    public async Task Handle_SameReactionType_RemovesAndDecrements()
    {
        var request = new ToggleReactionCommand { PostId = "post1", UserId = Guid.NewGuid(), ReactionType = ReactionType.Like };
        var post = new CommunityPostDto { Id = "post1" };
        var existing = new CommunityReactionDto { Type = ReactionType.Like };
        
        _postRepoMock.Setup(x => x.GetByIdAsync("post1", default)).ReturnsAsync(post);
        _reactionRepoMock.Setup(x => x.GetAsync("post1", request.UserId.ToString(), default)).ReturnsAsync(existing);
        _reactionRepoMock.Setup(x => x.RemoveAsync("post1", request.UserId.ToString(), default)).ReturnsAsync(true);

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.True(result);
        _reactionRepoMock.Verify(x => x.RemoveAsync("post1", request.UserId.ToString(), default), Times.Once);
        _postRepoMock.Verify(x => x.IncrementReactionCountAsync("post1", ReactionType.Like, -1, default), Times.Once);
    }

    [Fact]
    public async Task Handle_DifferentReactionType_SwapsAndAdjustsCounts()
    {
        var request = new ToggleReactionCommand { PostId = "post1", UserId = Guid.NewGuid(), ReactionType = ReactionType.Love };
        var post = new CommunityPostDto { Id = "post1" };
        var existing = new CommunityReactionDto { Type = ReactionType.Like };
        
        _postRepoMock.Setup(x => x.GetByIdAsync("post1", default)).ReturnsAsync(post);
        _reactionRepoMock.Setup(x => x.GetAsync("post1", request.UserId.ToString(), default)).ReturnsAsync(existing);
        _reactionRepoMock.Setup(x => x.UpdateTypeAsync("post1", request.UserId.ToString(), ReactionType.Love, default)).ReturnsAsync(true);

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.True(result);
        _reactionRepoMock.Verify(x => x.UpdateTypeAsync("post1", request.UserId.ToString(), ReactionType.Love, default), Times.Once);
        _postRepoMock.Verify(x => x.IncrementReactionCountAsync("post1", ReactionType.Like, -1, default), Times.Once);
        _postRepoMock.Verify(x => x.IncrementReactionCountAsync("post1", ReactionType.Love, 1, default), Times.Once);
    }
}
