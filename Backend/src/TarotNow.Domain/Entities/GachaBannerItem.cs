/*
 * ===================================================================
 * FILE: GachaBannerItem.cs
 * NAMESPACE: TarotNow.Domain.Entities
 * ===================================================================
 * MỤC ĐÍCH:
 *   Một mảnh nhỏ trong giỏ quà Gacha.
 * ===================================================================
 */

using System;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Mảnh rác vụn hay vàng khối rớt ra từ Banner Gacha.
/// </summary>
public class GachaBannerItem
{
    public Guid Id { get; private set; }
    public Guid BannerId { get; private set; }
    
    public GachaBanner? Banner { get; private set; }
    
    public string Rarity { get; private set; } = string.Empty;
    public string RewardType { get; private set; } = string.Empty;
    public string RewardValue { get; private set; } = string.Empty; // Số gold, kim cương hoặc Title Code.
    
    // Điểm trọng lượng (Basis points: 10000 = 100%)
    public int WeightBasisPoints { get; private set; }
    
    public string DisplayNameVi { get; private set; } = string.Empty;
    public string DisplayNameEn { get; private set; } = string.Empty;
    public string? DisplayIcon { get; private set; }

    protected GachaBannerItem() { }

    public GachaBannerItem(Guid bannerId, string rarity, string rewardType, string rewardValue, int weightBasisPoints, string displayNameVi, string displayNameEn, string? displayIcon = null)
    {
        Id = Guid.NewGuid();
        BannerId = bannerId;
        Rarity = rarity;
        RewardType = rewardType;
        RewardValue = rewardValue;
        WeightBasisPoints = weightBasisPoints;
        DisplayNameVi = displayNameVi;
        DisplayNameEn = displayNameEn;
        DisplayIcon = displayIcon;
    }
}
