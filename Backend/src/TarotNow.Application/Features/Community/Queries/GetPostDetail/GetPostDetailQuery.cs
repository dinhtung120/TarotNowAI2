

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Community;
using TarotNow.Application.Features.Community.Mappings;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Queries.GetPostDetail;

// Query lấy chi tiết một bài viết community.
public class GetPostDetailQuery : IRequest<CommunityPostFeedItemDto>
{
    // Định danh bài viết cần lấy.
    public string PostId { get; set; } = string.Empty;

    // Định danh người xem để kiểm tra quyền và enrich reaction.
    public Guid ViewerId { get; set; }
}

// Handler truy vấn chi tiết bài viết.
public class GetPostDetailQueryHandler : IRequestHandler<GetPostDetailQuery, CommunityPostFeedItemDto>
{
    private readonly ICommunityPostRepository _postRepo;
    private readonly ICommunityReactionRepository _reactionRepo;

    /// <summary>
    /// Khởi tạo handler get post detail.
    /// Luồng xử lý: nhận repository post/reaction để trả dữ liệu bài viết đã enrich reaction của viewer.
    /// </summary>
    public GetPostDetailQueryHandler(
        ICommunityPostRepository postRepo,
        ICommunityReactionRepository reactionRepo)
    {
        _postRepo = postRepo;
        _reactionRepo = reactionRepo;
    }

    /// <summary>
    /// Xử lý query lấy chi tiết bài viết.
    /// Luồng xử lý: tải post hợp lệ, kiểm tra quyền xem với post private, lấy reaction của viewer rồi map DTO trả về.
    /// </summary>
    public async Task<CommunityPostFeedItemDto> Handle(GetPostDetailQuery request, CancellationToken cancellationToken)
    {
        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
        {
            // Bài viết không tồn tại hoặc đã xóa.
            throw new NotFoundException("Bài viết không tồn tại hoặc đã bị xoá.");
        }

        var viewerId = request.ViewerId.ToString();
        if (post.Visibility == PostVisibility.Private && post.AuthorId != viewerId)
        {
            // Post private chỉ cho tác giả xem trong ngữ cảnh hiện tại.
            throw new ForbiddenException("Bạn không có quyền xem bài viết riêng tư này.");
        }

        var viewerReaction = await _reactionRepo.GetAsync(request.PostId, viewerId, cancellationToken);

        var mapped = CommunityPostFeedItemMapper.Map(post);
        mapped.ViewerReaction = viewerReaction?.Type;
        return mapped;
    }
}
