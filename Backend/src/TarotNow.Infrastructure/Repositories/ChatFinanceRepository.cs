/*
 * FILE: ChatFinanceRepository.cs
 * MỤC ĐÍCH: Repository quản lý tài chính chat (PostgreSQL).
 *   Quản lý 2 bảng: chat_finance_sessions + chat_question_items.
 *
 *   PHÂN BIỆT VỚI WalletRepository:
 *   → WalletRepo: Credit/Debit/Freeze/Release tiền thật (sổ cái).
 *   → ChatFinanceRepo: quản lý METADATA (session, câu hỏi, trạng thái, thời hạn).
 *   → Wallet operations (freeze/release/refund) PHẢI qua IWalletRepository.
 *
 *   CÁC NHÓM CHỨC NĂNG:
 *   1. SESSIONS: CRUD cho ChatFinanceSession (1 session per conversation)
 *   2. QUESTION ITEMS: CRUD cho ChatQuestionItem (mỗi câu hỏi trả phí = 1 item)
 *   3. TIMER QUERIES: cho Background Jobs (auto-cancel, auto-refund, auto-release)
 *
 *   LUỒNG ESCROW CHAT:
 *   → User gửi câu hỏi → freeze Diamond → tạo QuestionItem (status = pending)
 *   → Reader chấp nhận → status = accepted → Reader trả lời → replied_at set
 *   → Sau 24h không dispute → auto-release → Diamond chuyển cho Reader
 *   → Nếu Reader không trả lời sau 24h → auto-refund → Diamond hoàn User
 *   → Nếu offer hết hạn → auto-cancel → Diamond hoàn User
 */

using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;

namespace TarotNow.Infrastructure.Repositories;

/// <summary>
/// Implement IChatFinanceRepository — metadata tài chính chat (PostgreSQL).
/// </summary>
public class ChatFinanceRepository : IChatFinanceRepository
{
    private readonly ApplicationDbContext _db;

    public ChatFinanceRepository(ApplicationDbContext db) => _db = db;

    // ==================== SESSIONS ====================

    /// <summary>Tìm session theo ConversationRef (linkage MongoDB conversation → PG finance).</summary>
    public async Task<ChatFinanceSession?> GetSessionByConversationRefAsync(string conversationRef, CancellationToken ct = default)
        => await _db.ChatFinanceSessions.FirstOrDefaultAsync(s => s.ConversationRef == conversationRef, ct);

    /// <summary>Tìm session theo Primary Key.</summary>
    public async Task<ChatFinanceSession?> GetSessionByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.ChatFinanceSessions.FindAsync(new object[] { id }, ct);

    /// <summary>
    /// Lấy session với FOR UPDATE lock — dùng khi cập nhật status/tổng tiền.
    /// FOR UPDATE: khóa hàng để tránh 2 request đồng thời cập nhật cùng session.
    /// </summary>
    public async Task<ChatFinanceSession?> GetSessionForUpdateAsync(Guid id, CancellationToken ct = default)
    {
        var sessions = await _db.ChatFinanceSessions
            .FromSqlRaw("SELECT * FROM chat_finance_sessions WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(ct);
        return sessions.FirstOrDefault();
    }

    /// <summary>Tạo session mới (khi User bắt đầu conversation có trả phí).</summary>
    public async Task AddSessionAsync(ChatFinanceSession session, CancellationToken ct = default)
        => await _db.ChatFinanceSessions.AddAsync(session, ct);

    /// <summary>Cập nhật session (không SaveChanges — để TransactionCoordinator quản lý).</summary>
    public Task UpdateSessionAsync(ChatFinanceSession session, CancellationToken ct = default)
    {
        _db.ChatFinanceSessions.Update(session);
        return Task.CompletedTask;
    }

    // ==================== QUESTION ITEMS ====================

    /// <summary>Tìm câu hỏi theo Primary Key.</summary>
    public async Task<ChatQuestionItem?> GetItemByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.ChatQuestionItems.FindAsync(new object[] { id }, ct);

    /// <summary>Lấy câu hỏi với FOR UPDATE lock — dùng khi Reader accept/reply.</summary>
    public async Task<ChatQuestionItem?> GetItemForUpdateAsync(Guid id, CancellationToken ct = default)
    {
        var items = await _db.ChatQuestionItems
            .FromSqlRaw("SELECT * FROM chat_question_items WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(ct);
        return items.FirstOrDefault();
    }

    /// <summary>
    /// Tìm câu hỏi theo idempotency key — chặn User gửi trùng câu hỏi.
    /// Nếu key đã tồn tại → trả về item cũ, không tạo mới.
    /// </summary>
    public async Task<ChatQuestionItem?> GetItemByIdempotencyKeyAsync(string key, CancellationToken ct = default)
        => await _db.ChatQuestionItems.FirstOrDefaultAsync(i => i.IdempotencyKey == key, ct);

    /// <summary>Lấy tất cả câu hỏi của 1 session, sắp theo thời gian.</summary>
    public async Task<List<ChatQuestionItem>> GetItemsBySessionIdAsync(Guid sessionId, CancellationToken ct = default)
        => await _db.ChatQuestionItems
            .Where(i => i.FinanceSessionId == sessionId)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync(ct);

    /// <summary>Tạo câu hỏi mới (không SaveChanges — để TransactionCoordinator quản lý).</summary>
    public async Task AddItemAsync(ChatQuestionItem item, CancellationToken ct = default)
        => await _db.ChatQuestionItems.AddAsync(item, ct);

    /// <summary>Cập nhật câu hỏi (không SaveChanges).</summary>
    public Task UpdateItemAsync(ChatQuestionItem item, CancellationToken ct = default)
    {
        _db.ChatQuestionItems.Update(item);
        return Task.CompletedTask;
    }

    // ==================== TIMER QUERIES — Background Jobs ====================
    // Background Jobs chạy định kỳ, quét các item quá hạn để xử lý tự động.

    /// <summary>
    /// Lấy offer đã HẾT HẠN — auto-cancel.
    /// Điều kiện: status = pending + offer_expires_at < NOW()
    /// → User gửi offer nhưng Reader không accept trong thời hạn → hủy + hoàn tiền.
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetExpiredOffersAsync(CancellationToken ct = default)
        => await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Pending
                     && i.OfferExpiresAt != null
                     && i.OfferExpiresAt < DateTime.UtcNow)
            .ToListAsync(ct);

    /// <summary>
    /// Lấy item cần AUTO-REFUND — Reader nhận nhưng không trả lời sau 24h.
    /// Điều kiện: status = accepted + auto_refund_at < NOW() + replied_at IS NULL
    /// → Diamond trả lại User, Reader bị trừ uy tín.
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetItemsForAutoRefundAsync(CancellationToken ct = default)
        => await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Accepted
                     && i.AutoRefundAt != null
                     && i.AutoRefundAt < DateTime.UtcNow
                     && i.RepliedAt == null)
            .ToListAsync(ct);

    /// <summary>
    /// Lấy item cần AUTO-RELEASE — Reader đã trả lời, User không dispute sau 24h.
    /// Điều kiện: status = accepted + auto_release_at < NOW() + replied_at IS NOT NULL
    /// → Diamond chuyển cho Reader (thanh toán thành công).
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetItemsForAutoReleaseAsync(CancellationToken ct = default)
        => await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Accepted
                     && i.AutoReleaseAt != null
                     && i.AutoReleaseAt < DateTime.UtcNow
                     && i.RepliedAt != null)
            .ToListAsync(ct);

    /// <summary>Lưu thay đổi — gọi khi KHÔNG dùng TransactionCoordinator.</summary>
    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _db.SaveChangesAsync(ct);
}
