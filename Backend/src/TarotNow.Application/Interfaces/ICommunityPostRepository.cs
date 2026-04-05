/*
 * ===================================================================
 * FILE: ICommunityPostRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao diện kết nối với Kho lưu trữ Bài Viết Cộng Đồng.
 * ===================================================================
 */

using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

public interface ICommunityPostRepository
{
    /// <summary>Tạo mới một bài viết.</summary>
    Task<CommunityPostDto> CreateAsync(CommunityPostDto post, CancellationToken cancellationToken = default);

    /// <summary>Lấy chi tiết một bài viết theo ID.</summary>
    Task<CommunityPostDto?> GetByIdAsync(string postId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách bài viết phân trang (Feed).
    /// </summary>
    /// <param name="page">Trang hiện tại (1-based).</param>
    /// <param name="pageSize">Số lượng 1 trang.</param>
    /// <param name="authorId">Lọc theo tác giả (nếu cần xem trang cá nhân).</param>
    /// <param name="visibility">Lọc trạng thái hiển thị.</param>
    Task<(IEnumerable<CommunityPostDto> Items, long TotalCount)> GetFeedAsync(
        int page, int pageSize, string? viewerId = null, string? authorId = null, string? visibility = null, CancellationToken cancellationToken = default);

    /// <summary>Sửa nội dung bài viết.</summary>
    Task<bool> UpdateContentAsync(string postId, string content, CancellationToken cancellationToken = default);

    /// <summary>Xoá mềm (Soft-delete).</summary>
    Task<bool> SoftDeleteAsync(string postId, string deletedBy, CancellationToken cancellationToken = default);

    /// <summary>Tăng/Giảm biến đếm lượng biểu cảm (Dùng cho thao tác Reaction).</summary>
    Task IncrementReactionCountAsync(string postId, string reactionType, int delta, CancellationToken cancellationToken = default);

    /// <summary>Tăng/Giảm biến đếm lượng bình luận.</summary>
    Task IncrementCommentsCountAsync(string postId, int delta, CancellationToken cancellationToken = default);
}
