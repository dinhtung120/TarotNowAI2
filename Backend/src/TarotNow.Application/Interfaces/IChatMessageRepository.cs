using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository abstraction cho collection chat_messages (MongoDB).
///
/// Messages thuộc conversation — sort by created_at (timeline).
/// </summary>
public interface IChatMessageRepository
{
    /// <summary>Thêm tin nhắn mới.</summary>
    Task AddAsync(ChatMessageDto message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy tin nhắn theo conversation, phân trang.
    /// Sort by created_at DESC — tin mới nhất trước.
    /// Frontend sẽ reverse lại để hiển thị đúng timeline.
    /// </summary>
    Task<(IEnumerable<ChatMessageDto> Items, long TotalCount)> GetByConversationIdPaginatedAsync(
        string conversationId, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đánh dấu tất cả tin nhắn chưa đọc trong conversation là đã đọc.
    /// Trả về số tin nhắn đã cập nhật — dùng để giảm unread_count.
    /// </summary>
    Task<long> MarkAsReadAsync(string conversationId, string readerId, CancellationToken cancellationToken = default);
}
