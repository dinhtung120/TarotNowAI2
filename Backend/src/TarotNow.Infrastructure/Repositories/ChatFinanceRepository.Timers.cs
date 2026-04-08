using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Repositories;

// Partial truy vấn item đến hạn theo timer jobs của chat finance.
public partial class ChatFinanceRepository
{
    /// <summary>
    /// Lấy các offer đã hết hạn.
    /// Luồng xử lý: lọc status Pending, có OfferExpiresAt và nhỏ hơn thời điểm hiện tại UTC.
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetExpiredOffersAsync(CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Pending
                     && i.OfferExpiresAt != null
                     && i.OfferExpiresAt < DateTime.UtcNow)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Lấy các item đến hạn auto-refund.
    /// Luồng xử lý: item Accepted quá AutoRefundAt và chưa có phản hồi RepliedAt.
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetItemsForAutoRefundAsync(CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Accepted
                     && i.AutoRefundAt != null
                     && i.AutoRefundAt < DateTime.UtcNow
                     && i.RepliedAt == null)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Lấy các item đến hạn auto-release.
    /// Luồng xử lý: item Accepted quá AutoReleaseAt và đã có RepliedAt.
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetItemsForAutoReleaseAsync(CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Accepted
                     && i.AutoReleaseAt != null
                     && i.AutoReleaseAt < DateTime.UtcNow
                     && i.RepliedAt != null)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Lấy item disputed đến hạn auto-resolve.
    /// Luồng xử lý: lọc status Disputed và dùng mốc updated_at fallback created_at để so với dueAtUtc.
    /// </summary>
    public async Task<List<ChatQuestionItem>> GetDisputedItemsForAutoResolveAsync(DateTime dueAtUtc, CancellationToken ct = default)
    {
        return await _db.ChatQuestionItems
            .Where(i => i.Status == QuestionItemStatus.Disputed
                     && (i.UpdatedAt ?? i.CreatedAt) <= dueAtUtc)
            .ToListAsync(ct);
    }
}
