using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

// Partial thao tác finance session trong chat.
public partial class ChatFinanceRepository
{
    /// <summary>
    /// Lấy finance session theo conversationRef.
    /// Luồng xử lý: query bản ghi đầu tiên khớp conversation reference.
    /// </summary>
    public async Task<ChatFinanceSession?> GetSessionByConversationRefAsync(string conversationRef, CancellationToken ct = default)
    {
        return await _db.ChatFinanceSessions.FirstOrDefaultAsync(s => s.ConversationRef == conversationRef, ct);
    }

    /// <summary>
    /// Lấy finance session theo id.
    /// Luồng xử lý: dùng FindAsync theo khóa chính.
    /// </summary>
    public async Task<ChatFinanceSession?> GetSessionByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.ChatFinanceSessions.FindAsync(new object[] { id }, ct);
    }

    /// <summary>
    /// Lấy nhiều finance session theo danh sách conversationRefs.
    /// Luồng xử lý: filter In(conversationRef) và trả danh sách tương ứng.
    /// </summary>
    public async Task<List<ChatFinanceSession>> GetSessionsByConversationRefsAsync(IEnumerable<string> refs, CancellationToken ct = default)
    {
        return await _db.ChatFinanceSessions.Where(s => refs.Contains(s.ConversationRef)).ToListAsync(ct);
    }

    /// <summary>
    /// Lấy finance session theo id với khóa FOR UPDATE.
    /// Luồng xử lý: query raw SQL FOR UPDATE để bảo vệ cập nhật số dư/escrow khỏi race-condition.
    /// </summary>
    public async Task<ChatFinanceSession?> GetSessionForUpdateAsync(Guid id, CancellationToken ct = default)
    {
        var sessions = await _db.ChatFinanceSessions
            .FromSqlRaw("SELECT * FROM chat_finance_sessions WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(ct);
        return sessions.FirstOrDefault();
    }

    /// <summary>
    /// Thêm finance session mới.
    /// Luồng xử lý: add vào DbContext, SaveChanges do lớp gọi điều phối.
    /// </summary>
    public async Task AddSessionAsync(ChatFinanceSession session, CancellationToken ct = default)
    {
        await _db.ChatFinanceSessions.AddAsync(session, ct);
    }

    /// <summary>
    /// Cập nhật finance session.
    /// Luồng xử lý: mark modified trong DbContext để lưu ở transaction hiện tại.
    /// </summary>
    public Task UpdateSessionAsync(ChatFinanceSession session, CancellationToken ct = default)
    {
        _db.ChatFinanceSessions.Update(session);
        return Task.CompletedTask;
    }
}
