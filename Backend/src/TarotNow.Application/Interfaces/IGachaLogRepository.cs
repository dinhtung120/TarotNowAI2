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
    Task InsertLogAsync(Guid userId, string bannerCode, string rarity, string rewardType, string rewardValue, long spentDiamond, bool wasPity, string? rngSeed, DateTime createdAt, CancellationToken ct);
    Task<List<TarotNow.Application.Features.Gacha.Dtos.GachaHistoryItemDto>> GetUserLogsAsync(Guid userId, int limit, CancellationToken ct);
}
