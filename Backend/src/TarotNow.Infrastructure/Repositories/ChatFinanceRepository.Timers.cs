using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Repositories;

public partial class ChatFinanceRepository
{
    public async Task<List<ChatQuestionItem>> GetExpiredOffersAsync(CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Pending
                     && i.OfferExpiresAt != null
                     && i.OfferExpiresAt < DateTime.UtcNow)
            .ToListAsync(ct);
    }

    public async Task<List<ChatQuestionItem>> GetItemsForAutoRefundAsync(CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Accepted
                     && i.AutoRefundAt != null
                     && i.AutoRefundAt < DateTime.UtcNow
                     && i.RepliedAt == null)
            .ToListAsync(ct);
    }

    public async Task<List<ChatQuestionItem>> GetItemsForAutoReleaseAsync(CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Accepted
                     && i.AutoReleaseAt != null
                     && i.AutoReleaseAt < DateTime.UtcNow
                     && i.RepliedAt != null)
            .ToListAsync(ct);
    }

    public async Task<List<ChatQuestionItem>> GetDisputedItemsForAutoResolveAsync(DateTime dueAtUtc, CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Disputed
                     && (i.UpdatedAt ?? i.CreatedAt) <= dueAtUtc)
            .ToListAsync(ct);
    }
}
