
using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

// Contract quản lý bài viết cộng đồng để tách logic feed khỏi chi tiết lưu trữ.
public interface ICommunityPostRepository
{
    /// <summary>
    /// Tạo bài viết mới để ghi nhận nội dung do người dùng đăng.
    /// Luồng xử lý: persist DTO bài viết và trả bản ghi đã được tạo.
    /// </summary>
    Task<CommunityPostDto> CreateAsync(CommunityPostDto post, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy bài viết theo id để phục vụ xem chi tiết hoặc kiểm tra quyền chỉnh sửa.
    /// Luồng xử lý: truy vấn theo postId và trả null khi không tồn tại.
    /// </summary>
    Task<CommunityPostDto?> GetByIdAsync(string postId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy feed bài viết có phân trang theo người xem, tác giả và mức hiển thị.
    /// Luồng xử lý: áp bộ lọc feed, phân trang bằng page/pageSize và trả items kèm tổng số.
    /// </summary>
    Task<(IEnumerable<CommunityPostDto> Items, long TotalCount)> GetFeedAsync(
        int page, int pageSize, string? viewerId = null, string? authorId = null, string? visibility = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật nội dung bài viết khi tác giả chỉnh sửa thông tin.
    /// Luồng xử lý: tìm bài viết theo postId, ghi nội dung mới và trả kết quả thành công.
    /// </summary>
    Task<bool> UpdateContentAsync(string postId, string content, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa mềm bài viết để ẩn khỏi feed nhưng vẫn giữ lịch sử cho audit.
    /// Luồng xử lý: đánh dấu trạng thái xóa theo postId/deletedBy và trả true khi cập nhật được.
    /// </summary>
    Task<bool> SoftDeleteAsync(string postId, string deletedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tăng/giảm bộ đếm reaction theo loại cảm xúc để thống kê tương tác chính xác.
    /// Luồng xử lý: xác định postId và reactionType, cộng dồn theo delta.
    /// </summary>
    Task IncrementReactionCountAsync(string postId, string reactionType, int delta, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đổi reaction từ loại cũ sang loại mới trong một thao tác atomic.
    /// Luồng xử lý: giảm bộ đếm loại cũ và tăng bộ đếm loại mới trong cùng update command.
    /// </summary>
    Task SwapReactionCountAsync(string postId, string oldReactionType, string newReactionType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tăng/giảm bộ đếm bình luận của bài viết nhằm đồng bộ số liệu hiển thị.
    /// Luồng xử lý: cập nhật comments_count theo postId với giá trị delta.
    /// </summary>
    Task IncrementCommentsCountAsync(string postId, int delta, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật trạng thái attach media của bài viết.
    /// Luồng xử lý: set trạng thái và lỗi gần nhất (nếu có) cho postId mục tiêu.
    /// </summary>
    Task UpdateMediaAttachStatusAsync(
        string postId,
        string status,
        string? lastError = null,
        CancellationToken cancellationToken = default);
}
