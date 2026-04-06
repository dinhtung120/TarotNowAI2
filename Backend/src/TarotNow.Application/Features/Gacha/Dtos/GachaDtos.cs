namespace TarotNow.Application.Features.Gacha.Dtos;

public class GachaBannerDto
{
    public string Code { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? DescriptionVi { get; set; }
    public string? DescriptionEn { get; set; }
    public long CostDiamond { get; set; }
    public string OddsVersion { get; set; } = string.Empty;
    public int UserCurrentPity { get; set; }
}

public class GachaBannerOddsDto
{
    public string BannerCode { get; set; } = string.Empty;
    public string OddsVersion { get; set; } = string.Empty;
    public System.Collections.Generic.List<GachaBannerItemDto> Items { get; set; } = new();
}

public class GachaBannerItemDto
{
    public string Rarity { get; set; } = string.Empty;
    public string RewardType { get; set; } = string.Empty;
    public string RewardValue { get; set; } = string.Empty;
    public int WeightBasisPoints { get; set; } 
    public string DisplayNameVi { get; set; } = string.Empty;
    public string DisplayNameEn { get; set; } = string.Empty;
    public string? DisplayIcon { get; set; }
    public double ProbabilityPercent => WeightBasisPoints / 100.0;
}

public class GachaHistoryItemDto
{
    public string BannerCode { get; set; } = string.Empty;
    public string Rarity { get; set; } = string.Empty;
    public string RewardType { get; set; } = string.Empty;
    public string RewardValue { get; set; } = string.Empty;
    public long SpentDiamond { get; set; }
    public bool WasPityTriggered { get; set; }
    public System.DateTime CreatedAt { get; set; }
}
