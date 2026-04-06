/*
 * ===================================================================
 * FILE: GachaRewardLog.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Dấu tích Kế Toán Đỏ Lấy Hàng Gacha — Record chống gian lận Postgres.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Biên bản không thể tẩy xóa báo chứng khách đập đá hất tài chính vào Banner, rơi mảnh nào ghi hết ra.
/// IdempotencyKey ở đây chặn lũ clone HTTP Request chập nháy.
/// </summary>
public class GachaRewardLog
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid BannerId { get; private set; }
    public Guid BannerItemId { get; private set; }
    
    public string OddsVersion { get; private set; } = string.Empty;
    public long SpentDiamond { get; private set; }
    
    public string Rarity { get; private set; } = string.Empty;
    public string RewardType { get; private set; } = string.Empty;
    public string RewardValue { get; private set; } = string.Empty;
    
    public int PityCountAtSpin { get; private set; }
    public bool WasPityTriggered { get; private set; }
    
    // Dải hạt tinh khôi RNG Seed lôi ra để chứng minh "tao ko can thiệp số".
    public string? RngSeed { get; private set; }
    public string IdempotencyKey { get; private set; } = string.Empty;
    
    public DateTime CreatedAt { get; private set; }

    protected GachaRewardLog() { }

    private GachaRewardLog(GachaRewardLogCreateRequest request)
    {
        Id = Guid.NewGuid();
        UserId = request.UserId;
        BannerId = request.BannerId;
        BannerItemId = request.BannerItemId;
        OddsVersion = request.OddsVersion;
        SpentDiamond = request.SpentDiamond;
        Rarity = request.Rarity;
        RewardType = request.RewardType;
        RewardValue = request.RewardValue;
        PityCountAtSpin = request.PityCountAtSpin;
        WasPityTriggered = request.WasPityTriggered;
        RngSeed = request.RngSeed;
        IdempotencyKey = request.IdempotencyKey;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đẻ biên bản Kế toán tay
    /// </summary>
    public static GachaRewardLog Create(GachaRewardLogCreateRequest request)
    {
        return new GachaRewardLog(request);
    }
}

public sealed record GachaRewardLogCreateRequest(
    Guid UserId,
    Guid BannerId,
    Guid BannerItemId,
    string OddsVersion,
    long SpentDiamond,
    string Rarity,
    string RewardType,
    string RewardValue,
    int PityCountAtSpin,
    bool WasPityTriggered,
    string? RngSeed,
    string IdempotencyKey);
