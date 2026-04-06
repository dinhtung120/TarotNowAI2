/*
 * ===================================================================
 * FILE: GachaBanner.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Đại diện cho một Gacha Banner (Luật quay, chi phí, Pity).
 * ===================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Gacha Banner, chứa pool đồ, giá và luật chơi riêng biệt.
/// </summary>
public class GachaBanner
{
    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string NameVi { get; private set; } = string.Empty;
    public string NameEn { get; private set; } = string.Empty;
    public string? DescriptionVi { get; private set; }
    public string? DescriptionEn { get; private set; }
    
    // Yêu cầu chi phí là Diamond. Mặc định là 5 điểm.
    public long CostDiamond { get; private set; } = 5;
    
    // Phiên bản odds (ví dụ: v1.0). Đảm bảo hợp đồng công khai cho user check rate.
    public string OddsVersion { get; private set; } = string.Empty;
    
    // Banner sống lúc nào tới lúc nào (Sự kiện hay Standard).
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }
    
    // Hệ thống xót thương Pity.
    public bool PityEnabled { get; private set; } = true;
    public int HardPityCount { get; private set; } = 90;
    
    public bool IsActive { get; private set; } = true;
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Danh sách items mồi ngon 
    private readonly List<GachaBannerItem> _items = new();
    public IReadOnlyCollection<GachaBannerItem> Items => _items.AsReadOnly();

    protected GachaBanner() { } // Chống cháy EF Core

    public GachaBanner(string code, string nameVi, string nameEn, long costDiamond, string oddsVersion, DateTime effectiveFrom, DateTime? effectiveTo = null, bool pityEnabled = true, int hardPityCount = 90)
    {
        Id = Guid.NewGuid();
        Code = code;
        NameVi = nameVi;
        NameEn = nameEn;
        CostDiamond = costDiamond;
        OddsVersion = oddsVersion;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        PityEnabled = pityEnabled;
        HardPityCount = hardPityCount;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Check mớ ngày xem nó còn sống cho quay không.
    /// </summary>
    public bool IsCurrentlyActive()
    {
        if (!IsActive) return false;
        var now = DateTime.UtcNow;
        if (now < EffectiveFrom) return false;
        if (EffectiveTo.HasValue && now > EffectiveTo.Value) return false;
        return true;
    }

    /// <summary>
    /// Cộng dồn tổng trọng số phải = 10000 BP (100.00%)
    /// </summary>
    public bool ValidateOddsSum(IEnumerable<GachaBannerItem> itemsToValidate)
    {
        return itemsToValidate.Sum(x => x.WeightBasisPoints) == 10000;
    }
}
