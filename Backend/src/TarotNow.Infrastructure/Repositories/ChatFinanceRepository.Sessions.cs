using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

public partial class ChatFinanceRepository
{
    public async Task<ChatFinanceSession?> GetSessionByConversationRefAsync(string conversationRef, CancellationToken ct = default)
    {
        return await _db.ChatFinanceSessions.FirstOrDefaultAsync(s => s.ConversationRef == conversationRef, ct);
    }

    public async Task<ChatFinanceSession?> GetSessionByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.ChatFinanceSessions.FindAsync(new object[] { id }, ct);
    }

    public async Task<List<ChatFinanceSession>> GetSessionsByConversationRefsAsync(IEnumerable<string> refs, CancellationToken ct = default)
    {
        return await _db.ChatFinanceSessions.Where(s => refs.Contains(s.ConversationRef)).ToListAsync(ct);
    }

    public async Task<ChatFinanceSession?> GetSessionForUpdateAsync(Guid id, CancellationToken ct = default)
    {
        var sessions = await _db.ChatFinanceSessions
            .FromSqlRaw("SELECT * FROM chat_finance_sessions WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(ct);
        return sessions.FirstOrDefault();
    }

    public async Task AddSessionAsync(ChatFinanceSession session, CancellationToken ct = default)
    {
        await _db.ChatFinanceSessions.AddAsync(session, ct);
    }

    public Task UpdateSessionAsync(ChatFinanceSession session, CancellationToken ct = default)
    {
        _db.ChatFinanceSessions.Update(session);
        return Task.CompletedTask;
    }
}
