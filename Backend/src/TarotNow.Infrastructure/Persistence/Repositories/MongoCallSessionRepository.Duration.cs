using MongoDB.Driver;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý cập nhật duration khi phiên gọi kết thúc.
public partial class MongoCallSessionRepository
{
    /// <summary>
    /// Cập nhật thời lượng cuộc gọi khi trạng thái chuyển sang Ended.
    /// Luồng xử lý: kiểm tra điều kiện ended hợp lệ, tính duration theo giây và update vào document.
    /// </summary>
    private async Task UpdateDurationWhenEndedAsync(
        string id,
        CallSessionStatus newStatus,
        DateTime? endedAt,
        DateTime? startedAt,
        CancellationToken ct)
    {
        if (newStatus != CallSessionStatus.Ended || !endedAt.HasValue || !startedAt.HasValue)
        {
            return;
            // Edge case: chưa đủ dữ liệu mốc thời gian thì không thể tính duration an toàn.
        }

        var duration = Math.Max(0, (int)(endedAt.Value - startedAt.Value).TotalSeconds);
        // Chặn âm trong trường hợp clock skew hoặc dữ liệu mốc thời gian bất thường.

        var durationUpdate = Builders<CallSessionDocument>.Update.Set(x => x.DurationSeconds, duration);
        await _context.CallSessions.UpdateOneAsync(x => x.Id == id, durationUpdate, cancellationToken: ct);
    }
}
