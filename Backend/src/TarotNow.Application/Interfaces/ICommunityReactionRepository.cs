/*
 * ===================================================================
 * FILE: ICommunityReactionRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao diện kết nối với Kho lưu trữ Biểu Cảm Cộng Đồng (Reactions).
 * ===================================================================
 */

using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

public interface ICommunityReactionRepository
{
    /// <summary>
    /// Thêm reaction nếu chưa tồn tại (Idempotent upsert).
    /// Trả về true nếu thực sự thêm mới, false nếu đã có sẵn.
    /// </summary>
    Task<bool> AddOrIgnoreAsync(CommunityReactionDto reaction, CancellationToken cancellationToken = default);

    /// <summary>Xoá biểu cảm của một user trên một bài.</summary>
    Task<bool> RemoveAsync(string postId, string userId, CancellationToken cancellationToken = default);

    /// <summary>Lấy thông tin biểu cảm hiện hữu (để xem User đã thả loại nào).</summary>
    Task<CommunityReactionDto?> GetAsync(string postId, string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Chuyển đổi Reaction Type (VD từ Like sang Love).
    /// Tra về true nếu update thành công.
    /// </summary>
    Task<bool> UpdateTypeAsync(string postId, string userId, string newType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách Reaction Types của một User cho một list các bài Post (Dùng để tô màu icon trên bảng tin Feed).
    /// </summary>
    /// <returns>Dictionary với Key = PostId, Value = ReactionType</returns>
    Task<Dictionary<string, string>> GetUserReactionsForPostsAsync(string userId, IEnumerable<string> postIds, CancellationToken cancellationToken = default);
}
