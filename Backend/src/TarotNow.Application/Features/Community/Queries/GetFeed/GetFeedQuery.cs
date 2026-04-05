/*
 * ===================================================================
 * FILE: GetFeedQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Community.Queries.GetFeed
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lấy danh sách các bài viết phân trang (Feed) để hiển thị trên UI.
 *   Xử lý việc phân trang và join thêm thông tin Reaction của chính người 
 *   đang xem (Viewer) để frontend biết icon Like/Love đang sáng hay tắt.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Queries.GetFeed;

public class GetFeedQuery : IRequest<(IEnumerable<CommunityPostFeedItemDto> Items, long TotalCount)>
{
    public Guid ViewerId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? AuthorFilter { get; set; }
    public string? VisibilityFilter { get; set; }
}

public class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, (IEnumerable<CommunityPostFeedItemDto> Items, long TotalCount)>
{
    private readonly ICommunityPostRepository _postRepo;
    private readonly ICommunityReactionRepository _reactionRepo;

    public GetFeedQueryHandler(ICommunityPostRepository postRepo, ICommunityReactionRepository reactionRepo)
    {
        _postRepo = postRepo;
        _reactionRepo = reactionRepo;
    }

    public async Task<(IEnumerable<CommunityPostFeedItemDto> Items, long TotalCount)> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        // HIGH FIX #6: Giới hạn page/pageSize chống DoS
        if (request.Page < 1) request.Page = 1;
        if (request.PageSize < 1 || request.PageSize > 50) request.PageSize = 50;

        // 1. Quét kho lấy bài (Lọc Cờ Xóa, Phân quyền Public/Private...)
        var (posts, total) = await _postRepo.GetFeedAsync(
            request.Page, 
            request.PageSize, 
            request.ViewerId.ToString(),
            request.AuthorFilter, 
            request.VisibilityFilter, 
            cancellationToken);

        if (!posts.Any())
            return (Enumerable.Empty<CommunityPostFeedItemDto>(), total);

        var postIds = posts.Select(p => p.Id).ToList();

        // 2. Chắp vá thêm hồn: Xem Visitor này có Reaction gì với các post này chưa?
        var userReactions = await _reactionRepo.GetUserReactionsForPostsAsync(
            request.ViewerId.ToString(), 
            postIds, 
            cancellationToken);

        // 3. Xào chẻ DTO chuẩn hóa cho Frontend
        var feedItems = posts.Select(p => new CommunityPostFeedItemDto
        {
            Id = p.Id,
            AuthorId = p.AuthorId,
            AuthorDisplayName = p.AuthorDisplayName,
            AuthorAvatarUrl = p.AuthorAvatarUrl,
            Content = p.Content,
            Visibility = p.Visibility,
            ReactionsCount = p.ReactionsCount,
            TotalReactions = p.TotalReactions,
            CommentsCount = p.CommentsCount,
            IsDeleted = p.IsDeleted,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            // Nếu có thả thính thì gắn vào đây để UI bật cờ
            ViewerReaction = userReactions.ContainsKey(p.Id) ? userReactions[p.Id] : null
        });

        return (feedItems, total);
    }
}
