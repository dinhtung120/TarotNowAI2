using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

// Partial thao tác item trong chat finance session.
public partial class ChatFinanceRepository
{
    /// <summary>
    /// Lấy item theo id.
    /// Luồng xử lý: dùng FindAsync để truy vấn khóa chính nhanh.
    /// </summary>
    public async Task<ChatQuestionItem?> GetItemByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems.FindAsync(new object[] { id }, ct);
    }

    /// <summary>
    /// Lấy item theo id với khóa FOR UPDATE.
    /// Luồng xử lý: dùng raw SQL FOR UPDATE để chống race-condition khi nhiều luồng cùng cập nhật item.
    /// </summary>
    public async Task<ChatQuestionItem?> GetItemForUpdateAsync(Guid id, CancellationToken ct = default)
    {
        var items = await _db.ChatQuestionItems
            .FromSqlRaw("SELECT * FROM chat_question_items WHERE id = {0} FOR UPDATE", id)
            .ToListAsync(ct);
        return items.FirstOrDefault();
    }

    /// <summary>
    /// Lấy item theo idempotency key.
    /// Luồng xử lý: query bản ghi đầu tiên khớp key để phục vụ xử lý retry an toàn.
    /// </summary>
    public async Task<ChatQuestionItem?> GetItemByIdempotencyKeyAsync(string key, CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems.FirstOrDefaultAsync(i => i.IdempotencyKey == key, ct);
    }

    /// <summary>
    /// Lấy toàn bộ items thuộc một finance session.
    /// Luồng xử lý: lọc theo FinanceSessionId và sort CreatedAt tăng dần để giữ timeline hỏi/đáp.
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetItemsBySessionIdAsync(Guid sessionId, CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.FinanceSessionId == sessionId)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Thêm item mới vào DbContext.
    /// Luồng xử lý: chỉ add entity, SaveChanges được điều phối ở lớp gọi.
    /// </summary>
    public async Task AddItemAsync(ChatQuestionItem item, CancellationToken ct = default)
    {
        await _db.ChatQuestionItems.AddAsync(item, ct);
    }

    /// <summary>
    /// Cập nhật item hiện có trong DbContext.
    /// Luồng xử lý: mark entity modified, SaveChanges do lớp gọi quyết định để gộp transaction.
    /// </summary>
    public Task UpdateItemAsync(ChatQuestionItem item, CancellationToken ct = default)
    {
        _db.ChatQuestionItems.Update(item);
        return Task.CompletedTask;
    }
}
