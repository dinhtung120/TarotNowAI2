using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract quản lý review reader theo từng conversation completed.
/// </summary>
public interface IConversationReviewRepository
{
    /// <summary>
    /// Lấy review theo cặp conversation và user.
    /// </summary>
    Task<ConversationReviewDto?> GetByConversationAndUserAsync(
        string conversationId,
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Thử thêm review mới; trả false nếu đã tồn tại bản ghi review trùng.
    /// </summary>
    Task<bool> TryAddAsync(ConversationReviewDto review, CancellationToken cancellationToken = default);
}
