
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Contract lưu nhật ký gacha để truy vết kết quả quay và phục vụ lịch sử người dùng.
public interface IGachaLogRepository
{
    /// <summary>
    /// Ghi một log quay gacha mới sau khi xác định phần thưởng.
    /// Luồng xử lý: nhận request đã chuẩn hóa và persist vào kho log.
    /// </summary>
    Task InsertLogAsync(GachaLogInsertRequest request, CancellationToken ct);

    /// <summary>
    /// Lấy lịch sử quay của người dùng để hiển thị các lần quay gần đây.
    /// Luồng xử lý: lọc theo userId, giới hạn theo limit và trả danh sách item lịch sử.
    /// </summary>
    Task<List<TarotNow.Application.Features.Gacha.Dtos.GachaHistoryItemDto>> GetUserLogsAsync(Guid userId, int limit, CancellationToken ct);
}

// Request ghi log quay gacha nhằm lưu đầy đủ dữ liệu đối soát RNG và phần thưởng.
public sealed record GachaLogInsertRequest(
    Guid UserId,
    string BannerCode,
    string Rarity,
    string RewardType,
    string RewardValue,
    long SpentDiamond,
    bool WasPity,
    string? RngSeed,
    DateTime CreatedAt);
