




namespace TarotNow.Api.Contracts;

public class UpdateProfileRequest
{
    
    public string DisplayName { get; set; } = string.Empty;

    
    public string? AvatarUrl { get; set; }

    
    public DateTime DateOfBirth { get; set; }
}

public class CreateDepositOrderRequest
{
    
    public long AmountVnd { get; set; }
}

public class RecordConsentRequest
{
    
    public string DocumentType { get; set; } = string.Empty;

    
    public string Version { get; set; } = string.Empty;
}

public class UpdatePromotionRequest
{
    
    public long MinAmountVnd { get; set; }

    
    public long BonusDiamond { get; set; }

    
    public bool IsActive { get; set; }
}
