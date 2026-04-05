/*
 * ===================================================================
 * FILE: GetPostDetailQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Community.Queries.GetPostDetail
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lấy chi tiết 1 bài viết cộng đồng trọn vẹn, kèm Reaction của Viewer.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Queries.GetPostDetail;

public class GetPostDetailQuery : IRequest<CommunityPostFeedItemDto>
{
    public string PostId { get; set; } = string.Empty;
    public Guid ViewerId { get; set; }
}

public class GetPostDetailQueryHandler : IRequestHandler<GetPostDetailQuery, CommunityPostFeedItemDto>
{
    private readonly ICommunityPostRepository _postRepo;
    private readonly ICommunityReactionRepository _reactionRepo;

    public GetPostDetailQueryHandler(ICommunityPostRepository postRepo, ICommunityReactionRepository reactionRepo)
    {
        _postRepo = postRepo;
        _reactionRepo = reactionRepo;
    }

    public async Task<CommunityPostFeedItemDto> Handle(GetPostDetailQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
            throw new NotFoundException("Bài viết không tồn tại hoặc đã bị xoá.");

        var viewerReaction = await _reactionRepo.GetAsync(request.PostId, request.ViewerId.ToString(), cancellationToken);

        return new CommunityPostFeedItemDto
        {
            Id = post.Id,
            AuthorId = post.AuthorId,
            AuthorDisplayName = post.AuthorDisplayName,
            AuthorAvatarUrl = post.AuthorAvatarUrl,
            Content = post.Content,
            Visibility = post.Visibility,
            ReactionsCount = post.ReactionsCount,
            TotalReactions = post.TotalReactions,
            CommentsCount = post.CommentsCount,
            IsDeleted = post.IsDeleted,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            ViewerReaction = viewerReaction?.Type
        };
    }
}
