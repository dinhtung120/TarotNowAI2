using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Repositories;

// Partial truy vấn dữ liệu dispute trong chat finance.
public partial class ChatFinanceRepository
{
    /// <summary>
    /// Lấy danh sách item đang disputed có phân trang.
    /// Luồng xử lý: chuẩn hóa page/pageSize, lọc status=Disputed, đếm tổng rồi lấy trang theo updated_at/created_at giảm dần.
    /// </summary>
    public async Task<(IReadOnlyList<ChatQuestionItem> Items, long TotalCount)> GetDisputedItemsPaginatedAsync(
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);
        // Giới hạn page size để dashboard tranh chấp luôn phản hồi ổn định.

        var query = _db.ChatQuestionItems.Where(i => i.Status == QuestionItemStatus.Disputed);
        var totalCount = await query.LongCountAsync(ct);

        var items = await query
            .OrderByDescending(i => i.UpdatedAt ?? i.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Đếm số dispute gần đây theo receiver.
    /// Luồng xử lý: lọc receiverId và disputeWindowStart từ mốc fromUtc trở đi.
    /// </summary>
    public Task<long> CountRecentDisputesByReceiverAsync(Guid receiverId, DateTime fromUtc, CancellationToken ct = default)
    {
        return _db.ChatQuestionItems.LongCountAsync(
            i => i.ReceiverId == receiverId
                 && i.DisputeWindowStart != null
                 && i.DisputeWindowStart >= fromUtc,
            ct);
    }
}
