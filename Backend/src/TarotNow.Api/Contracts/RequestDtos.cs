namespace TarotNow.Api.Contracts;

/// <summary>
/// DTO yêu cầu cập nhật hồ sơ người dùng.
/// Tách riêng khỏi ProfileController để tuân thủ nguyên tắc mỗi file chỉ chứa một class.
/// </summary>
public class UpdateProfileRequest
{
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
}

/// <summary>
/// DTO yêu cầu tạo đơn nạp tiền.
/// Tách riêng khỏi DepositController.
/// </summary>
public class CreateDepositOrderRequest
{
    public long AmountVnd { get; set; }
}

/// <summary>
/// DTO yêu cầu ghi nhận đồng thuận pháp lý.
/// Tách riêng khỏi LegalController.
/// </summary>
public class RecordConsentRequest
{
    public string DocumentType { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}

/// <summary>
/// DTO yêu cầu cập nhật chương trình khuyến mãi.
/// Tách riêng khỏi PromotionsController.
/// </summary>
public class UpdatePromotionRequest
{
    public long MinAmountVnd { get; set; }
    public long BonusDiamond { get; set; }
    public bool IsActive { get; set; }
}
