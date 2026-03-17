using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository abstraction cho collection conversations (MongoDB).
///
/// Conversations = header của chat 1-1 giữa User và Reader.
/// Mỗi conversation gắn 1:1 với finance_session (PostgreSQL).
/// </summary>
public interface IConversationRepository
{
    /// <summary>Tạo conversation mới.</summary>
    Task AddAsync(ConversationDto conversation, CancellationToken cancellationToken = default);

    /// <summary>Lấy conversation theo ObjectId.</summary>
    Task<ConversationDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy conversation giữa 2 user cụ thể (chưa completed/cancelled).
    /// Dùng để kiểm tra đã có conversation active chưa — tránh duplicate.
    /// </summary>
    Task<ConversationDto?> GetActiveByParticipantsAsync(
        string userId, string readerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inbox của user — danh sách conversations phân trang.
    /// Sort by last_message_at DESC (tin mới nhất trước).
    /// </summary>
    Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByUserIdPaginatedAsync(
        string userId, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inbox của reader — danh sách conversations phân trang.
    /// </summary>
    Task<(IEnumerable<ConversationDto> Items, long TotalCount)> GetByReaderIdPaginatedAsync(
        string readerId, int page, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>Cập nhật conversation (status, last_message_at, unread_count).</summary>
    Task UpdateAsync(ConversationDto conversation, CancellationToken cancellationToken = default);
}
