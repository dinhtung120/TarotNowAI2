using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Repository cho escrow tables: chat_finance_sessions + chat_question_items (PostgreSQL).
///
/// Tất cả thao tác wallet (freeze/release/refund) PHẢI qua IWalletRepository.
/// Repository này chỉ quản lý session/item metadata.
/// </summary>
public interface IChatFinanceRepository
{
    // --- Sessions ---
    Task<ChatFinanceSession?> GetSessionByConversationRefAsync(string conversationRef, CancellationToken ct = default);
    Task<ChatFinanceSession?> GetSessionByIdAsync(Guid id, CancellationToken ct = default);
    Task AddSessionAsync(ChatFinanceSession session, CancellationToken ct = default);
    Task UpdateSessionAsync(ChatFinanceSession session, CancellationToken ct = default);

    // --- Question Items ---
    Task<ChatQuestionItem?> GetItemByIdAsync(Guid id, CancellationToken ct = default);
    Task<ChatQuestionItem?> GetItemByIdempotencyKeyAsync(string key, CancellationToken ct = default);
    Task<List<ChatQuestionItem>> GetItemsBySessionIdAsync(Guid sessionId, CancellationToken ct = default);
    Task AddItemAsync(ChatQuestionItem item, CancellationToken ct = default);
    Task UpdateItemAsync(ChatQuestionItem item, CancellationToken ct = default);

    // --- Timer Queries (Background Jobs) ---
    /// <summary>Pending offers quá hạn → auto-cancel.</summary>
    Task<List<ChatQuestionItem>> GetExpiredOffersAsync(CancellationToken ct = default);

    /// <summary>Accepted + no reply after 24h → auto-refund.</summary>
    Task<List<ChatQuestionItem>> GetItemsForAutoRefundAsync(CancellationToken ct = default);

    /// <summary>Replied + no dispute after 24h → auto-release.</summary>
    Task<List<ChatQuestionItem>> GetItemsForAutoReleaseAsync(CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
