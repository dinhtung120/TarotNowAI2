namespace TarotNow.Infrastructure.Options;

// Options phiên bản tài liệu pháp lý áp dụng hiện hành.
public sealed class LegalSettingsOptions
{
    // Phiên bản Terms of Service.
    public string TOSVersion { get; set; } = "1.0";

    // Phiên bản Privacy Policy.
    public string PrivacyVersion { get; set; } = "1.0";

    // Phiên bản AI disclaimer.
    public string AiDisclaimerVersion { get; set; } = "1.0";
}
