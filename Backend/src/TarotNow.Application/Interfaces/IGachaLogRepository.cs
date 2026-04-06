/*
 * ===================================================================
 * FILE: IGachaLogRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao diện kết nối MongoDB cho Lịch sử Gacha Analytics/Audit.
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

// Thay vì import MongoDocument ngay đây, ta dùng interface chung để App Domain không phụ thuộc vào Infrastructure chi tiết.
// Ta định nghĩa 1 DTO gọn trong Layer App.
public interface IGachaLogRepository
{
    Task InsertLogAsync(GachaLogInsertRequest request, CancellationToken ct);
    Task<List<TarotNow.Application.Features.Gacha.Dtos.GachaHistoryItemDto>> GetUserLogsAsync(Guid userId, int limit, CancellationToken ct);
}

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
