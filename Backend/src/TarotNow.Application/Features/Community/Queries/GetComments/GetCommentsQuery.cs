using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Community.Queries.GetComments;

// Query lấy danh sách bình luận theo bài viết.
public class GetCommentsQuery : IRequest<(IEnumerable<CommunityCommentDto> Items, long TotalCount)>
{
    // Định danh bài viết cần lấy bình luận.
    public required string PostId { get; set; }

    // Định danh người xem (tùy chọn) để kiểm tra quyền với post private.
    public Guid? ViewerId { get; set; }

    // Trang hiện tại của danh sách comment.
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 10;
}

// Handler truy vấn bình luận của community post.
public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, (IEnumerable<CommunityCommentDto> Items, long TotalCount)>
{
    private readonly ICommunityCommentRepository _commentRepo;
    private readonly ICommunityPostRepository _postRepo;

    /// <summary>
    /// Khởi tạo handler get comments.
    /// Luồng xử lý: nhận repository comment/post để kiểm tra quyền và truy vấn dữ liệu phân trang.
    /// </summary>
    public GetCommentsQueryHandler(ICommunityCommentRepository commentRepo, ICommunityPostRepository postRepo)
    {
        _commentRepo = commentRepo;
        _postRepo = postRepo;
    }

    /// <summary>
    /// Xử lý query lấy bình luận.
    /// Luồng xử lý: chuẩn hóa paging, kiểm tra bài viết tồn tại + quyền truy cập, rồi trả danh sách bình luận phân trang.
    /// </summary>
    public async Task<(IEnumerable<CommunityCommentDto> Items, long TotalCount)> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        if (request.Page < 1)
        {
            // Chuẩn hóa page tối thiểu để tránh truy vấn ngoài phạm vi.
            request.Page = 1;
        }

        if (request.PageSize < 1 || request.PageSize > 50)
        {
            // Giới hạn page size theo rule để bảo vệ hiệu năng.
            request.PageSize = 50;
        }

        var post = await _postRepo.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null || post.IsDeleted)
        {
            // Không thể lấy comment cho post không tồn tại hoặc đã xóa.
            throw new NotFoundException("Bài viết không tồn tại.");
        }

        if (post.Visibility == PostVisibility.Private
            && post.AuthorId != request.ViewerId?.ToString())
        {
            // Post private chỉ cho chủ bài viết xem bình luận trong ngữ cảnh hiện tại.
            throw new ForbiddenException("Không có quyền xem bình luận bài viết riêng tư.");
        }

        return await _commentRepo.GetByPostIdAsync(request.PostId, request.Page, request.PageSize, cancellationToken);
    }
}
