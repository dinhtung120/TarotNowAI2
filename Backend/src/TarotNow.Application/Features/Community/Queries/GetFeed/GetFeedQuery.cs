

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Features.Community.Mappings;

namespace TarotNow.Application.Features.Community.Queries.GetFeed;

// Query lấy feed community theo phân trang và bộ lọc.
public class GetFeedQuery : IRequest<(IEnumerable<CommunityPostFeedItemDto> Items, long TotalCount)>
{
    // Định danh người xem feed (dùng để enrich viewer reaction).
    public Guid ViewerId { get; set; }

    // Trang hiện tại.
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 10;

    // Bộ lọc theo tác giả (tùy chọn).
    public string? AuthorFilter { get; set; }

    // Bộ lọc visibility (tùy chọn).
    public string? VisibilityFilter { get; set; }
}

// Handler truy vấn community feed.
public class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, (IEnumerable<CommunityPostFeedItemDto> Items, long TotalCount)>
{
    // Ràng buộc trang tối thiểu.
    private const int MinPage = 1;

    // Ràng buộc page size tối thiểu.
    private const int MinPageSize = 1;

    // Ràng buộc page size tối đa.
    private const int MaxPageSize = 50;

    private readonly ICommunityPostRepository _postRepo;
    private readonly ICommunityReactionRepository _reactionRepo;

    /// <summary>
    /// Khởi tạo handler get feed.
    /// Luồng xử lý: nhận repository post/reaction để tải dữ liệu feed rồi enrich viewer reaction.
    /// </summary>
    public GetFeedQueryHandler(
        ICommunityPostRepository postRepo,
        ICommunityReactionRepository reactionRepo)
    {
        _postRepo = postRepo;
        _reactionRepo = reactionRepo;
    }

    /// <summary>
    /// Xử lý query lấy feed.
    /// Luồng xử lý: chuẩn hóa paging, tải post feed theo filter, lấy reaction của viewer theo batch, map DTO và gán viewer reaction.
    /// </summary>
    public async Task<(IEnumerable<CommunityPostFeedItemDto> Items, long TotalCount)> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        NormalizePaging(request);

        var (posts, total) = await _postRepo.GetFeedAsync(
            request.Page,
            request.PageSize,
            request.ViewerId.ToString(),
            request.AuthorFilter,
            request.VisibilityFilter,
            cancellationToken);

        if (!posts.Any())
        {
            // Không có post thì trả danh sách rỗng cùng total từ repository.
            return (Enumerable.Empty<CommunityPostFeedItemDto>(), total);
        }

        var postIds = posts.Select(p => p.Id).ToList();
        var userReactions = await _reactionRepo.GetUserReactionsForPostsAsync(
            request.ViewerId.ToString(),
            postIds,
            cancellationToken);

        var feedItems = MapFeedItems(posts, userReactions);
        return (feedItems, total);
    }

    /// <summary>
    /// Chuẩn hóa tham số phân trang cho feed query.
    /// Luồng xử lý: ép Page về tối thiểu 1 và ép PageSize vào ngưỡng cho phép.
    /// </summary>
    private static void NormalizePaging(GetFeedQuery request)
    {
        if (request.Page < MinPage)
        {
            request.Page = MinPage;
        }

        if (request.PageSize < MinPageSize || request.PageSize > MaxPageSize)
        {
            request.PageSize = MaxPageSize;
        }
    }

    /// <summary>
    /// Map danh sách post sang feed item và gán reaction của viewer.
    /// Luồng xử lý: map từng post bằng helper thống nhất, rồi tra dictionary reaction để gán ViewerReaction.
    /// </summary>
    private IEnumerable<CommunityPostFeedItemDto> MapFeedItems(
        IEnumerable<CommunityPostDto> posts,
        IReadOnlyDictionary<string, string> userReactions)
    {
        return posts.Select(post =>
        {
            var mapped = CommunityPostFeedItemMapper.Map(post);
            var postId = post.Id;
            mapped.ViewerReaction = userReactions.ContainsKey(postId)
                // Có reaction của viewer thì gán đúng type để UI highlight.
                ? userReactions[postId]
                : null;
            return mapped;
        });
    }
}
