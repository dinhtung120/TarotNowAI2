using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

// Adapter cung cấp phiên bản pháp lý đã chuẩn hóa cho tầng nghiệp vụ.
public sealed class LegalVersionSettings : ILegalVersionSettings
{
    /// <summary>
    /// Khởi tạo phiên bản điều khoản pháp lý từ cấu hình.
    /// Luồng này ép fallback mặc định để API luôn trả về version hợp lệ.
    /// </summary>
    public LegalVersionSettings(IOptions<LegalSettingsOptions> options)
    {
        var configured = options.Value;
        // Chuẩn hóa từng phiên bản để tránh giá trị trống gây sai lệch consent của người dùng.
        TOSVersion = ResolveVersion(configured.TOSVersion);
        PrivacyVersion = ResolveVersion(configured.PrivacyVersion);
        AiDisclaimerVersion = ResolveVersion(configured.AiDisclaimerVersion);
    }

    // Phiên bản Terms of Service hiện hành.
    public string TOSVersion { get; }
    // Phiên bản Privacy Policy hiện hành.
    public string PrivacyVersion { get; }
    // Phiên bản AI disclaimer hiện hành.
    public string AiDisclaimerVersion { get; }

    /// <summary>
    /// Chuẩn hóa chuỗi version pháp lý.
    /// Luồng fallback về "1.0" giúp hệ thống không phát sinh null/trống ở contract.
    /// </summary>
    private static string ResolveVersion(string? configuredValue)
    {
        return string.IsNullOrWhiteSpace(configuredValue) ? "1.0" : configuredValue.Trim();
    }
}
