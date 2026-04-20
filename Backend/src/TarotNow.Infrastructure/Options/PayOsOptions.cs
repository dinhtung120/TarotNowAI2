namespace TarotNow.Infrastructure.Options;

// Cấu hình credential PayOS từ appsettings/env.
public sealed class PayOsOptions
{
    // PayOS Client Id.
    public string ClientId { get; set; } = string.Empty;

    // PayOS API key.
    public string ApiKey { get; set; } = string.Empty;

    // PayOS checksum key dùng ký payload.
    public string ChecksumKey { get; set; } = string.Empty;

    // PayOS partner code (tùy chọn).
    public string PartnerCode { get; set; } = string.Empty;
}
