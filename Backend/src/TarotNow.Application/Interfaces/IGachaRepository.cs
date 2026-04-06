/*
 * ===================================================================
 * FILE: IGachaRepository.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao diện kết nối Database (PostgreSQL) cho Gacha Banner, Items, và Log Trúng Thưởng.
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface IGachaRepository
{
    // Banner management
    Task<GachaBanner?> GetActiveBannerAsync(string bannerCode, CancellationToken ct);
    Task<List<GachaBanner>> GetAllActiveBannersAsync(CancellationToken ct);
    Task<List<GachaBannerItem>> GetBannerItemsAsync(Guid bannerId, CancellationToken ct);
    
    // Pity tracking
    Task<int> GetUserPityCountAsync(Guid userId, Guid bannerId, CancellationToken ct);
    
    // Reward logging (PostgreSQL — source of truth)
    Task<GachaRewardLog> LogRewardAsync(GachaRewardLog log, CancellationToken ct);
    Task<bool> IdempotencyKeyExistsAsync(string key, CancellationToken ct);
    
    // Dành cho Idempotency trả về kết quả cũ (hỗ trợ 10x)
    Task<List<GachaRewardLog>> GetRewardLogsByIdempotencyKeyAsync(string key, CancellationToken ct);
}
