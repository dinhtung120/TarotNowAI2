using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

// Contract thao tác bình luận cộng đồng để quản lý vòng đời comment theo bài viết.
public interface ICommunityCommentRepository
{
    /// <summary>
    /// Thêm bình luận mới vào bài viết để ghi nhận tương tác người dùng.
    /// Luồng xử lý: persist comment đầu vào và trả bản ghi đã tạo.
    /// </summary>
    Task<CommunityCommentDto> AddCommentAsync(CommunityCommentDto comment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách bình luận theo bài viết có phân trang để hiển thị theo trang.
    /// Luồng xử lý: lọc theo postId, áp page/pageSize và trả items cùng tổng số.
    /// </summary>
    Task<(IEnumerable<CommunityCommentDto> Items, long TotalCount)> GetByPostIdAsync(string postId, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy bình luận theo id để phục vụ chỉnh sửa, xóa hoặc kiểm duyệt.
    /// Luồng xử lý: truy vấn theo commentId và trả null khi không tìm thấy.
    /// </summary>
    Task<CommunityCommentDto?> GetByIdAsync(string commentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa mềm bình luận để ẩn nội dung nhưng vẫn giữ lịch sử phục vụ audit.
    /// Luồng xử lý: đánh dấu deleted theo commentId và deletedBy, trả true khi cập nhật thành công.
    /// </summary>
    Task<bool> SoftDeleteAsync(string commentId, string deletedBy, CancellationToken cancellationToken = default);
}
