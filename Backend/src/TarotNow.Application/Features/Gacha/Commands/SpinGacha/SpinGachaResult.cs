namespace TarotNow.Application.Features.Gacha.Commands.SpinGacha;

public class SpinGachaResult
{
    public bool Success { get; set; }
    public bool IsIdempotentReplay { get; set; }
    
    // Pity properties sau khi quay xong
    public int CurrentPityCount { get; set; }
    public int HardPityThreshold { get; set; }
    public bool WasPityTriggered { get; set; }

    public List<SpinGachaItemResult> Items { get; set; } = new();
}

public class SpinGachaItemResult
{
    public string Rarity { get; set; } = string.Empty;
    public string RewardType { get; set; } = string.Empty;
    public string RewardValue { get; set; } = string.Empty;
    
    public string DisplayNameVi { get; set; } = string.Empty;
    public string DisplayNameEn { get; set; } = string.Empty;
    public string? DisplayIcon { get; set; }
}
