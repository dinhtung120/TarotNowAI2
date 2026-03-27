using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

public partial class ChatFinanceRepository
{
    public async Task<ChatQuestionItem?> GetItemByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems.FindAsync(new object[] { id }, ct);
    }

    public async Task<ChatQuestionItem?> GetItemForUpdateAsync(Guid id, CancellationToken ct = default)
    {
        var items = await _db.ChatQuestionItems
            .FromSqlRaw("SELECT * FROM chat_question_items WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(ct);
        return items.FirstOrDefault();
    }

    public async Task<ChatQuestionItem?> GetItemByIdempotencyKeyAsync(string key, CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems.FirstOrDefaultAsync(i => i.IdempotencyKey == key, ct);
    }

    public async Task<List<ChatQuestionItem>> GetItemsBySessionIdAsync(Guid sessionId, CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.FinanceSessionId == sessionId)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task AddItemAsync(ChatQuestionItem item, CancellationToken ct = default)
    {
        await _db.ChatQuestionItems.AddAsync(item, ct);
    }

    public Task UpdateItemAsync(ChatQuestionItem item, CancellationToken ct = default)
    {
        _db.ChatQuestionItems.Update(item);
        return Task.CompletedTask;
    }
}
