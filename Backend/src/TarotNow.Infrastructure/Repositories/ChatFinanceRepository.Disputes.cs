using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Repositories;

public partial class ChatFinanceRepository
{
    public async Task<(IReadOnlyList<ChatQuestionItem> Items, long TotalCount)> GetDisputedItemsPaginatedAsync(
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var query = _db.ChatQuestionItems.Where(i => i.Status == QuestionItemStatus.Disputed);
        var totalCount = await query.LongCountAsync(ct);

        var items = await query
            .OrderByDescending(i => i.UpdatedAt ?? i.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public Task<long> CountRecentDisputesByReceiverAsync(Guid receiverId, DateTime fromUtc, CancellationToken ct = default)
    {
        return _db.ChatQuestionItems.LongCountAsync(
            i => i.ReceiverId == receiverId
                 && i.DisputeWindowStart != null
                 && i.DisputeWindowStart >= fromUtc,
            ct);
    }
}
