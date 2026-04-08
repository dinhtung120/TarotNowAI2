
using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

// Contract quản lý phản ứng bài viết để đảm bảo mỗi người dùng có trạng thái reaction rõ ràng.
public interface ICommunityReactionRepository
{
    /// <summary>
    /// Thêm reaction mới hoặc bỏ qua khi đã tồn tại để tránh ghi trùng dữ liệu.
    /// Luồng xử lý: kiểm tra khóa duy nhất user/post trước khi chèn, trả false nếu đã có.
    /// </summary>
    Task<bool> AddOrIgnoreAsync(CommunityReactionDto reaction, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa reaction của người dùng trên bài viết khi hủy tương tác.
    /// Luồng xử lý: tìm theo cặp postId/userId và xóa bản ghi nếu tồn tại.
    /// </summary>
    Task<bool> RemoveAsync(string postId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy reaction hiện tại của người dùng trên một bài viết để quyết định thao tác kế tiếp.
    /// Luồng xử lý: truy vấn theo postId/userId và trả null nếu chưa từng reaction.
    /// </summary>
    Task<CommunityReactionDto?> GetAsync(string postId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cập nhật loại reaction khi người dùng đổi cảm xúc trên cùng bài viết.
    /// Luồng xử lý: định vị reaction theo postId/userId, đổi sang newType và trả kết quả cập nhật.
    /// </summary>
    Task<bool> UpdateTypeAsync(string postId, string userId, string newType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy map reaction của người dùng cho nhiều bài viết để render feed hiệu quả.
    /// Luồng xử lý: batch query theo userId và danh sách postIds, trả Dictionary postId -> reactionType.
    /// </summary>
    Task<Dictionary<string, string>> GetUserReactionsForPostsAsync(string userId, IEnumerable<string> postIds, CancellationToken cancellationToken = default);
}
