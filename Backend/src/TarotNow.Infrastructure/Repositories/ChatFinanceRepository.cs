using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation cho IChatFinanceRepository.
///
/// Quản lý chat_finance_sessions + chat_question_items (PostgreSQL).
/// Wallet operations (freeze/release/refund) PHẢI qua IWalletRepository (stored procedures).
/// Repository này chỉ quản lý metadata.
/// </summary>
public class ChatFinanceRepository : IChatFinanceRepository
{
    private readonly ApplicationDbContext _db;

    public ChatFinanceRepository(ApplicationDbContext db) => _db = db;

    // ======================================================================
    // SESSIONS
    // ======================================================================

    public async Task<ChatFinanceSession?> GetSessionByConversationRefAsync(string conversationRef, CancellationToken ct = default)
        => await _db.ChatFinanceSessions.FirstOrDefaultAsync(s => s.ConversationRef == conversationRef, ct);

    public async Task<ChatFinanceSession?> GetSessionByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.ChatFinanceSessions.FindAsync(new object[] { id }, ct);

    public async Task AddSessionAsync(ChatFinanceSession session, CancellationToken ct = default)
        => await _db.ChatFinanceSessions.AddAsync(session, ct);

    public Task UpdateSessionAsync(ChatFinanceSession session, CancellationToken ct = default)
    {
        _db.ChatFinanceSessions.Update(session);
        return Task.CompletedTask;
    }

    // ======================================================================
    // QUESTION ITEMS
    // ======================================================================

    public async Task<ChatQuestionItem?> GetItemByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.ChatQuestionItems.FindAsync(new object[] { id }, ct);

    public async Task<ChatQuestionItem?> GetItemByIdempotencyKeyAsync(string key, CancellationToken ct = default)
        => await _db.ChatQuestionItems.FirstOrDefaultAsync(i => i.IdempotencyKey == key, ct);

    public async Task<List<ChatQuestionItem>> GetItemsBySessionIdAsync(Guid sessionId, CancellationToken ct = default)
        => await _db.ChatQuestionItems
            .Where(i => i.FinanceSessionId == sessionId)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync(ct);

    public async Task AddItemAsync(ChatQuestionItem item, CancellationToken ct = default)
        => await _db.ChatQuestionItems.AddAsync(item, ct);

    public Task UpdateItemAsync(ChatQuestionItem item, CancellationToken ct = default)
    {
        _db.ChatQuestionItems.Update(item);
        return Task.CompletedTask;
    }

    // ======================================================================
    // TIMER QUERIES — cho Background Jobs
    // ======================================================================

    /// <summary>
    /// Pending items quá hạn offer → auto-cancel.
    /// offer_expires_at < NOW() AND status = 'pending'
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetExpiredOffersAsync(CancellationToken ct = default)
        => await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Pending
                     && i.OfferExpiresAt != null
                     && i.OfferExpiresAt < DateTime.UtcNow)
            .ToListAsync(ct);

    /// <summary>
    /// Accepted items + reader không reply sau 24h → auto-refund.
    /// auto_refund_at < NOW() AND status = 'accepted' AND replied_at IS NULL
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetItemsForAutoRefundAsync(CancellationToken ct = default)
        => await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Accepted
                     && i.AutoRefundAt != null
                     && i.AutoRefundAt < DateTime.UtcNow
                     && i.RepliedAt == null)
            .ToListAsync(ct);

    /// <summary>
    /// Replied items + no dispute sau 24h → auto-release.
    /// auto_release_at < NOW() AND status = 'accepted' AND replied_at IS NOT NULL
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetItemsForAutoReleaseAsync(CancellationToken ct = default)
        => await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Accepted
                     && i.AutoReleaseAt != null
                     && i.AutoReleaseAt < DateTime.UtcNow
                     && i.RepliedAt != null)
            .ToListAsync(ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _db.SaveChangesAsync(ct);
}
